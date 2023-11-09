import authorizenet from 'authorizenet';

export const processAuthorizeNet = {
	POST: async (req, res) => {
		const db = req.db;
		const apiLogin = await db.tournSetting.findOne({
			where : {
				tourn: req.body?.tourn,
				tag  : 'authorizenet_api_login',
			},
		});
		const transactionKey = await db.tournSetting.findOne({
			where : {
				tourn: req.body?.tourn,
				tag  : 'authorizenet_transaction_key',
			},
		});
		const orderData = req.body?.orderData;
		const payerName = `${req.body?.customerInformation.firstName} ${req.body?.customerInformation.lastName}`;
		const payerEmail = orderData.payerEmail;

		const dataDescriptor = req.body?.opaqueData?.dataDescriptor;
		const dataValue = req.body?.opaqueData?.dataValue;

		const APIContracts = authorizenet.APIContracts;
		const APIControllers = authorizenet.APIControllers;

		const merchantAuthenticationType = new APIContracts.MerchantAuthenticationType();
		merchantAuthenticationType.setName(apiLogin.dataValues.value);
		merchantAuthenticationType.setTransactionKey(transactionKey.dataValues.value);

		const opaqueData = new APIContracts.OpaqueDataType();
		opaqueData.setDataDescriptor(dataDescriptor);
		opaqueData.setDataValue(dataValue);

		const paymentType = new APIContracts.PaymentType();
		paymentType.setOpaqueData(opaqueData);

		const orderDetails = new APIContracts.OrderType();
		orderDetails.setDescription('Entry Fees');

		const billTo = new APIContracts.CustomerAddressType();
		billTo.setFirstName('Ellen');
		billTo.setLastName('Johnson');
		billTo.setCompany('Souveniropolis');
		billTo.setAddress('14 Main Street');
		billTo.setCity('Pecan Springs');
		billTo.setState('TX');
		billTo.setZip('44628');
		billTo.setCountry('USA');

		const lineItemId1 = new APIContracts.LineItemType();
		lineItemId1.setItemId('1');
		lineItemId1.setName('Entry Fee');
		lineItemId1.setDescription('Entry Fee');
		lineItemId1.setQuantity('1');
		lineItemId1.setUnitPrice(100.00);

		const lineItemList = [];
		lineItemList.push(lineItemId1);

		const lineItems = new APIContracts.ArrayOfLineItem();
		lineItems.setLineItem(lineItemList);

		const transactionSetting1 = new APIContracts.SettingType();
		transactionSetting1.setSettingName('duplicateWindow');
		transactionSetting1.setSettingValue('120');

		const transactionSetting2 = new APIContracts.SettingType();
		transactionSetting2.setSettingName('recurringBilling');
		transactionSetting2.setSettingValue('false');

		const transactionSettingList = [];
		transactionSettingList.push(transactionSetting1);
		transactionSettingList.push(transactionSetting2);

		const transactionSettings = new APIContracts.ArrayOfSetting();
		transactionSettings.setSetting(transactionSettingList);

		const transactionRequestType = new APIContracts.TransactionRequestType();
		transactionRequestType.setTransactionType(
			APIContracts.TransactionTypeEnum.AUTHCAPTURETRANSACTION
		);
		transactionRequestType.setPayment(paymentType);
		transactionRequestType.setAmount('100.00');
		transactionRequestType.setLineItems(lineItems);
		transactionRequestType.setOrder(orderDetails);
		transactionRequestType.setBillTo(billTo);
		transactionRequestType.setTransactionSettings(transactionSettings);

		const createRequest = new APIContracts.CreateTransactionRequest();
		createRequest.setMerchantAuthentication(merchantAuthenticationType);
		createRequest.setTransactionRequest(transactionRequestType);

		// console.log(JSON.stringify(createRequest.getJSON(), null, 2));

		const ctrl = new APIControllers.CreateTransactionController(createRequest.getJSON());

		// Defaults to sandbox
		// ctrl.setEnvironment(authorizenet.Constants.endpoint.production);

		ctrl.execute(async () => {
			const apiResponse = ctrl.getResponse();
			const response = new APIContracts.CreateTransactionResponse(apiResponse);

			// console.log(JSON.stringify(response, null, 2));

			if (response != null) {
				if (response.getMessages().getResultCode() === APIContracts.MessageTypeEnum.OK) {
					if (response.getTransactionResponse().getMessages() != null) {
						console.log(`Successfully created transaction with Transaction ID: ${response.getTransactionResponse().getTransId()}`);
						console.log(`Response Code: ${response.getTransactionResponse().getResponseCode()}`);
						console.log(`Message Code: ${response.getTransactionResponse().getMessages().getMessage()[0].getCode()}`);
						console.log(`Description: ${response.getTransactionResponse().getMessages().getMessage()[0].getDescription()}`);

						const paymentObject = {
							reason    : `PayPal Payment from ${payerName} ${payerEmail}`,
							amount    : parseFloat(orderData.purchase_units[0]?.amount.value) * -1,
							school    : orderData.purchase_units[0]?.custom_id,
							payment   : true,
							levied_at : Date(orderData.create_time),
							levied_by : orderData.person_id,
						};

						const payPalObject = {
							reason    : `Online Payment Processing Fee`,
							amount    : parseFloat(orderData.payment_fee),
							school    : orderData.purchase_units[0]?.custom_id,
							payment   : false,
							levied_at : Date(orderData.create_time),
							levied_by : orderData.person_id,
						};

						await db.fine.create(paymentObject);
						await db.fine.create(payPalObject);
					} else {
						console.log('Failed Transaction.');
						if (response.getTransactionResponse().getErrors() != null) {
							console.log(`Error Code: ${response.getTransactionResponse().getErrors().getError()[0].getErrorCode()}`);
							console.log(`Error message: ${response.getTransactionResponse().getErrors().getError()[0].getErrorText()}`);
						}
					}
				} else {
					console.log('Failed Transaction. ');
					if (response.getTransactionResponse() != null
						&& response.getTransactionResponse().getErrors() != null) {

						console.log(`Error Code: ${response.getTransactionResponse().getErrors().getError()[0].getErrorCode()}`);
						console.log(`Error message: ${response.getTransactionResponse().getErrors().getError()[0].getErrorText()}`);
					} else {
						console.log(`Error Code: ${response.getMessages().getMessage()[0].getCode()}`);
						console.log(`Error message: ${response.getMessages().getMessage()[0].getText()}`);
					}
				}
			} else {
				console.log('Null Response.');
			}
		});

		return res.status(200).json({ message: 'Transaction processed' });
	},
};

processAuthorizeNet.POST.apiDoc = {
	summary: 'Process a payment through Authorize',
	operationId: 'processAuthorizeNet',
	requestBody: {
		description: 'The order details',
		required: true,
		content: { '*/*': { schema: { type: 'object' } } },
	},
	responses: {
		200: {
			description: 'Authorize payment',
			content: {
				'*/*': {
					schema: {
						type: 'string',
					},
				},
			},
		},
		default: { $ref: '#/components/responses/ErrorResponse' },
	},
};

export default processAuthorizeNet;
