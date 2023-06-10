	/* confirmation dialog */
	$('.noenter').on('keyup keypress', function(e) {
		var keyCode = e.keyCode || e.which;
		if (keyCode === 13) {
			e.preventDefault();
			return false;
		}
	});

	/* copy text */
	/* this method of doing it, horking it in a hidden element, seems
	 * needlessly baroque but apparently that's the JS way of doing this. How
	 * stupid. */

	function copyToClipboard(elementId, textLabel) {
	    var $temp = $("<input>");
		$("body").append($temp);

		$temp.val($("#"+elementId).text()).select();
		document.execCommand("copy");
		$temp.remove();

		if (!textLabel) {
			textLabel = "Text ";
		}
		alertify.notify(textLabel+" copied to clipboard", "custom");
	}

	function confirmSubmit(message, submitButton) {

	    event.preventDefault();

	    alertify.confirm(
			"Please Confirm",
			message,
			function(event) {
				if (event) {
					var form = $(submitButton).closest('form');
					form.submit();
				} else {
					alertify.error("Save cancelled");
				}
    		},
			function() {
				alertify.error("Save cancelled");
			}
		);
	};

	function confirmAction(link, message, dest, payload) {

		alertify.confirm(
			"Are you sure?",
			message,
			function(event) {
				if (event) {
					if (
						typeof(link) === "object"
						&& link.submit
					) {
						link.submit();
					} else if (
						typeof(link) === 'function'
						&& typeof(dest) != 'undef'
						&& typeof(payload) != 'undef'
					) {
                        link(dest,payload);

                    } else {
					    window.location.href = link.href;
                    }
				}
			},
			function(event) {
				if (event) {
					alertify.error("Canceled");
				}
			}
		);
		return false;
	}

/* Respond to switch calls */

	function postEnter(e, checkObject, replyUrl) {
		if (e.keyCode == 13) {
			postSwitch(checkObject, replyUrl);
			return false;
		}
		return true;
	}

	function postConfirm(alertMessage, checkObject, replyUrl) {
		alertify.confirm("Please confirm", alertMessage, function(e) {
			if (e) {
				postSwitch(checkObject, replyUrl);
				return;
			} else {
				return;
			}
		}, function(no) { return; } );
		return;
	}

	(function(old) {
	  $.fn.attrs = function() {
		if(arguments.length === 0) {
		  if(this.length === 0) {
			return null;
		  }

		  var obj = {};
		  $.each(this[0].attributes, function() {
			if(this.specified) {
			  obj[this.name] = this.value;
			}
		  });
		  return obj;
		}

		return old.apply(this, arguments);
	  };
	})($.fn.attrs);

	function postSwitch(checkObject, replyUrl, callback, confirmMessage) {

		if (confirmMessage != undefined && confirmMessage != "") {
			alertify.confirm("Please confirm", confirmMessage, function(e) {
				if (e) {
				} else {
					return;
				}
			}, function(no) { return; } );
		}

		var attributes = {};
		attributes = $(checkObject).attrs();

		if (attributes.property_value === undefined) {
			attributes.property_value = $(checkObject).attr("value");
		}

		if (attributes.property_value === undefined) {
			attributes.property_value = $(checkObject).val();
		}

		if (checkObject.type === "checkbox") {
			if ($(checkObject).prop("checked") === false) {
				attributes.property_value = 0;
			}
		}

		if (attributes.parent_id === undefined) {
			attributes.parent_id = $(checkObject).parent().attr('id');
		}

		// I hate myself a lot sometimes.

		if ($(checkObject).attr("other_value")) {
			var otherObjectId = $(checkObject).attr("other_value");
			attributes.other_value = $("#"+otherObjectId).val();
		}

		// OK, most of the time.

		if ($(checkObject).attr("other_other_value")) {
			var otherObjectId = $(checkObject).attr("other_other_value");
			attributes.other_other_value = $("#"+otherObjectId).val();
		}

		if ($(checkObject).attr("other_text")) {
			var otherTextId = $(checkObject).attr("other_text");
			attributes.other_text = $("#"+otherTextId).val();
		}

		// Really, when don't I?

		if (attributes.option_one) {
			var optionOneId = $(checkObject).attr("option_one");

			if (optionOneId) {
				if ($("#"+optionOneId).prop("checked")) {
					attributes.option_one = true;
				}
			}

			if (attributes.option_one !== true) {
				delete attributes.option_one;
			}
		}

		if (attributes.option_two) {
			var optionTwoId = $(checkObject).attr("option_two");

			if (optionTwoId) {
				if ($("#"+optionTwoId).prop("checked")) {
					attributes.option_two = true;
				}
			}

			if (attributes.option_two !== true) {
				delete attributes.option_two;
			}
		}

		var accessType = "";

		if (attributes.post_method === "get") {
			accessType = "GET";
		} else if (attributes.post_method === "delete") {
			accessType = "DELETE";
		} else if (attributes.post_method === "put") {
			accessType = "PUT";
		} else {
			accessType = "POST";
		}

		$.ajax({
			type    : accessType,
			url     : replyUrl,
			data    : attributes,
			success : function(data, status, object, newCallback) {

				if (data) {

					if (data.reply) {

						if (attributes.reply_target) {
							$("#"+attributes.reply_target).text(data.reply);
						}

						if (attributes.reply_append) {
							$("#"+attributes.reply_append).append(data.reply);
						}

						if (data.reply_target) {
							$("#"+data.reply_target).text(data.reply);
						}

						if (data.reply_append) {
							$("#"+data.reply_append).append(data.reply);
						}

						$(".replybucket").text(data.reply);
						$(".replyappend").append(data.reply);
					}

					if (data.error) {

						alertify.error(data.message);
						console.log(data);

						if (data.destroy) {
							$("#"+data.destroy).remove();
							$("."+data.destroy).remove();
						}

						if (data.errSetValue) {
							data.errSetValue.forEach( function(item) {
								$("#"+item.id).val(item.content);
							});
						}

						if (data.errReplace) {
							data.errReplace.forEach( function(item) {
								if (item.destroy) {
									$("#"+item.id).remove();
								} else if (item.content) {
									$("#"+item.id).html(item.content);
								}
							});
						}

						if (data.refresh) {
							window.location.reload();
						}

					} else if (data.message) {

						alertify.dismissAll();
						alertify.notify(data.message, "custom");

						if (data.destroy) {
							$("#"+data.destroy).remove();
							$("."+data.destroy).remove();
						}

						if (data.showAll) {
							$("."+data.showAll).removeClass("hidden");
						}

						if (data.hideAll) {
							$("."+data.hideAll).addClass("hidden");
						}

						if (data.reveal) {
							$("#"+data.reveal).removeClass("hidden");
						}

						if (data.hide) {
							$("#"+data.hide).addClass("hidden");
						}

						if (attributes.on_success === "destroy") {
							$("#"+attributes.target_id).remove();
						} else if (attributes.on_success === "hide") {
							$("#"+attributes.target_id).addClass("hidden");
						} else if (
							attributes.on_success === "refresh"
							|| attributes.on_success === "reload"
							|| data.refresh
						) {
							window.location.reload();
						}

						if (attributes.new_parent) {
							$("#"+attributes.target_id).prependTo("#"+attributes.new_parent);
						}

						if (data.newParent) {
							$("#"+attributes.target_id).prependTo("#"+data.newParent);
						}

						if (data.setvalue) {
							data.setvalue.forEach( function(item) {
								$("#"+item.id).val(item.content);
							});
						}

						if (data.replace) {
							data.replace.forEach( function(item) {
								if (item.destroy) {
									$("#"+item.id).remove();
								} else if (item.content) {
									$("#"+item.id).html(item.content);
								}
							});
						}

						if (data.reclass) {
							data.reclass.forEach( function(item) {
								if (item.removeClass) {
									$("#"+item.id).removeClass(item.removeClass);
								}
								if (item.addClass) {
									$("#"+item.id).addClass(item.addClass);
								}
							});
						}

						if (data.reprop) {
							data.reprop.forEach( function(item) {
								$("#"+item.id).attr(item.property, item.value);
							});
						}

						if (data.norefresh) {

						} else {
							$('table').trigger('applyWidgets');
							$('table').trigger('update', [true]);
							fixVisual();
						}

					} else {
						console.log(data);
						alertify.warning("An error condition was tripped.");
					}

					if (callback && callback != 'false') {
						callback(data);
					}
					if (newCallback && newCallback != 'false') {
						newCallback(data);
					}
					return;
				}
			}
		});

		fixVisual();
		return;
	}

	function valueConfirm(alertMessage, value, url, callback) {
		alertify.confirm("Please confirm", alertMessage, function(e) {
			if (e) {
				postValue(value, url, callback);
				return;
			} else {
				return;
			}
		}, function(no) { return; } );
		return;
	}

	function postValue(value, url, callback) {

		var data = { 'value': value };

		$.ajax({
			type    : 'POST',
			url,
			data,
			success : function(data, status, object, newCallback) {

				if (data) {

					if (data.reply) {
						if (data.reply_target) {
							$("#"+data.reply_target).text(data.reply);
						}

						if (data.reply_append) {
							$("#"+data.reply_append).append(data.reply);
						}

						$(".replybucket").text(data.reply);
						$(".replyappend").append(data.reply);
					}

					if (data.error) {

						alertify.error(data.message);
						console.log(data);

						if (data.destroy) {
							$("#"+data.destroy).remove();
							$("."+data.destroy).remove();
						}

						if (data.errSetValue) {
							data.errSetValue.forEach( function(item) {
								$("#"+item.id).val(item.content);
							});
						}

						if (data.errReplace) {
							data.errReplace.forEach( function(item) {
								if (item.destroy) {
									$("#"+item.id).remove();
								} else if (item.content) {
									$("#"+item.id).html(item.content);
								}
							});
						}

						if (data.refresh) {
							window.location.reload();
						}

					} else if (data.message) {

						alertify.dismissAll();
						alertify.notify(data.message, "custom");

						if (data.destroy) {
							$("#"+data.destroy).remove();
							$("."+data.destroy).remove();
						}

						if (data.showAll) {
							$("."+data.showAll).removeClass("hidden");
						}

						if (data.hideAll) {
							$("."+data.hideAll).addClass("hidden");
						}

						if (data.reveal) {
							$("#"+data.reveal).removeClass("hidden");
						}

						if (data.hide) {
							$("#"+data.hide).addClass("hidden");
						}

						if (data.setvalue) {
							data.setvalue.forEach( function(item) {
								$("#"+item.id).val(item.content);
							});
						}

						if (data.replace) {
							data.replace.forEach( function(item) {
								if (item.destroy) {
									$("#"+item.id).remove();
								} else if (item.content) {
									$("#"+item.id).html(item.content);
								}
							});
						}

						if (data.reclass) {
							data.reclass.forEach( function(item) {
								if (item.removeClass) {
									$("#"+item.id).removeClass(item.removeClass);
								}
								if (item.addClass) {
									$("#"+item.id).addClass(item.addClass);
								}
							});
						}

						if (data.reprop) {
							data.reprop.forEach( function(item) {
								$("#"+item.id).attr(item.property, item.value);
							});
						}

						if (data.norefresh) {

						} else {
							$('table').trigger('applyWidgets');
							$('table').trigger('update', [true]);
							fixVisual();
						}

					} else {
						console.log(data);
						alertify.warning("An error condition was tripped.");
					}

					if (newCallback && newCallback != 'false') {
						newCallback(data);
					}

					if (callback && callback != 'false') {
						callback(data);
					}
					return;
				}
			}
		});
	}



	function pullUrl(targetUrl) {
		$.ajax({
			type    : 'GET',
			url     : targetUrl,
			success : function() {
				location.reload();
			}
		});
	}

/* zebra stripe the rows */

	function zebraRows() {

		var tags = ['.row', '.lightrow'];
		var parents = ['div', 'span', 'table'];

		tags.forEach(function(Tag) {

			parents.forEach(function(Parent) {

				$(document).find(Tag).parent().closest(Parent)
						.find(".row:visible:even").removeClass("odd");

				$(document).find(Tag).parent().closest(Parent)
						.find(".row:visible:even").addClass("even");

				$(document).find(Tag).parent().closest(Parent)
						.find(".row:visible:odd").removeClass("even");

				$(document).find(Tag).parent().closest(Parent)
						.find(".row:visible:odd").addClass("odd");
			});
		});
	};

/* Fix all the visual elements at once */
	function fixVisual() {
		$('table').trigger('applyWidgets');
		$('table').trigger('update', [true]);

		resizeAll();
		zebraRows();

		var base_url = window.location.origin.split(':').slice(1);
		var base_domain =  base_url[0].split('.').slice(-2).join('.');
		base_domain = base_domain.replace(/\//g,'');

		if (
			(base_domain !== 'tabroom.com')
			&& (base_domain !== 'debatefail.com')
			&& (base_domain !== 'tabroom.gay')
		) {
			pleaseStop();
		}
	}

	/* jerks */

	function pleaseStop() {
		$(".main").html('<h3 class="centeralign redtext">This site is not Tabroom. It is a phishing attempt</h3><h4 class="centeralign">Fortunately it was not a very good one, so Chris Palmer was able to mess with them a little and put this message up instead</h4><h4 class="centeralign bluetext">Real Tabroom will always say "https://www.tabroom.com" in the address bar.</h4><h5 class="centeralign">Also, it would not have worked anyway, for reasons I will not get into here</h5><p class="centeralign bluetext bigger martop">But seriously people, stop trying to make my life more stressful and stop doing things like this.  There are no fortunes to be made here, just a more tired Tabroom developer.</p>');
		console.log('no seriously stop');
	}

/* Change the file uploader div to show the name of the uploaded file */

	function uploadName(inputBox) {
		var fileTag = $(inputBox).attr("id");
		var fileName = $("#"+fileTag).val().replace(/C:\\fakepath\\/i, '');
		$("."+fileTag).html(fileName);
		$("."+fileTag).html(fileName);
	}


function uploaderName(uploader, filedisplay, sizeLimit) {

	if (uploader == null) {
		uploader = 'upload';
	}

	if (filedisplay == null) {
		filedisplay = 'filename';
	}

	if (sizeLimit == null) {
		sizeLimit = 15;
	}

	var filename = document.getElementById(uploader).value;
	var lastIndex = filename.lastIndexOf("\\");
	if (lastIndex >= 0) {
		filename = filename.substring(lastIndex + 1);
	}

	if (sizeLimit > 0 && typeof FileReader !== "undefined") {
		var size = document.getElementById(uploader).files[0].size;

		if (size > (parseInt(sizeLimit) * 1024 * 1024)) {

			document.getElementById(uploader).value = "";

			var sizeString = (size / 1024 / 1024).toFixed(1);

			var message = `File ${filename} size is ${sizeString} MB and the
				upload limit is ${sizeLimit} MB. Please compress or reformat the file,
				because otherwise upload and validation errors are very likely.`;

			alertify.confirm(
				"File size too large!",
				message,
				function(event) {
					alertify.error("File upload cleared");
				},
				function() {
					alertify.error("File upload cleared");
				}
			);
		}
	}

	document.getElementById(filedisplay).innerHTML = filename;
}

/* Registers the ballot entry shortcuts for win/loss */

function autoWin(input,e,aff,neg,affid,negid) {

	// if ($("#toggleKeyboardShortcuts").prop("checked") === false) {
	// 	return;
	// }

    var keyCode = e.keyCode;
    var filter = [0,8,9,16,17,18,37,38,39,40,46];
	var acceptable = ["a","A",1,"p","P","g","G",3,"n","N","c","C","o","O"];

	if (!containsElement(acceptable,input.value)) {
		input.value = null;
	}

	if (!containsElement(filter,keyCode)) {

		// All the various ways of designating one or the other winner.

        if (
			input.value == "A"
			|| input.value == "a"
			|| input.value == 1
			|| input.value == "p"
			|| input.value == "P"
			|| input.value == "g"
			|| input.value == "G"
		){

			input.value = aff;

			$('.aff').show();
			$('.aff_entry').addClass("winner_row");

			$('.neg').hide();
			$('.neg_entry').removeClass("winner_row");

			var winner = document.getElementById("winner");
			winner.value = affid;

			lowPointWin(1);
			changeFocus(input);
		}

        if (
			input.value == "N"
			|| input.value == "n"
			|| input.value == 3
			|| input.value == "c"
			|| input.value == "C"
			|| input.value == "o"
			|| input.value == "O"
		) {
			input.value = neg;

			$('.neg').show();
			$('.neg_entry').addClass("winner_row");

			$('.aff').hide();
			$('.aff_entry').removeClass("winner_row");

			var winner = document.getElementById("winner");
			winner.value = negid;
			lowPointWin(2);
			changeFocus(input);
		}

	}

	function lowPointWin(side) {

		var neg_points = document.getElementById("points_2").value;
		var neg_ranks = document.getElementById("ranks_2").value;

		var aff_points = document.getElementById("points_1").value;
		var aff_ranks = document.getElementById("ranks_1").value;

		if (side == 2) {

			if (aff_points > neg_points) {
				$('.lpw').show();
			}

			if (neg_points > aff_points) {
				$('.lpw').hide();
			}

		}

		if (side == 1) {

			if (neg_points > aff_points) {
				$('.lpw').show();
			}

			if (neg_points < aff_points) {
				$('.lpw').hide();
			}

		}

	}

    function containsElement(arr, ele) {
        var found = false, index = 0;
        while(!found && index < arr.length)
        if(arr[index] == ele)
        found = true;
        else
        index++;
        return found;
    }

	function changeFocus(input) {

		var next_index = getIndex(input) + 1;
        while(! input.form[next_index].tabIndex) {
			next_index = next_index + 1;
			if (next_index > input.form.length) break;
		}
       	input.form[next_index].focus();
	}

    return true;
}

/* Does the various shortcuts to register speaker point entry, like 285 = 28.5,
 * etc */

 function autoPoints(input,len,e,side,ratio,nototal,step) {

	if ($("#toggleKeyboardShortcuts").prop("checked") === false) {
		return;
	}

	var minPoints = $(input).attr("min");
	var maxPoints = $(input).attr("max");
	var pointStep = $(input).attr("step");

	if (nototal) {
		totalPoints = function() {
			return;
		}
	}

    var keyCode = e.keyCode;
    var filter = [0,8,16,17,18,37,38,39,40,46];

	if (pointStep === ".5"
		&& minPoints > -5
		&& maxPoints < 5
		&& input.value == 5
	) {

		input.value = .5;
		changeFocus(input);
		totalPoints(side,ratio);

	} else if (pointStep === ".1"
		&& minPoints >= 20
		&& maxPoints == 30
		&& input.value.length >= 2
		&& !containsElement(filter,keyCode)
	) {

        if (input.value == 't3') {
			input.value = 23 * 1;
			changeFocus(input);
			totalPoints(side,ratio);
		} else if (input.value != input.value * 1) {
			input.value = "";
		} else if (input.value != 30) {
			var number = input.value;
			number = number * 1;
			number = number / 10;
			number = number + 20;
			input.value = number;
			changeFocus(input);
			totalPoints(side,ratio);
		} else if (input.value == 30) {
			changeFocus(input);
			totalPoints(side,ratio);
		}

	} else if (pointStep === ".1"
		&& minPoints < 20
		&& maxPoints == 30
		&& input.value.length >= 3
		&& !containsElement(filter,keyCode)
	) {

		var number = input.value;
		number = number * 1;
		number = number / 10;

		input.value = number;
		changeFocus(input);
		totalPoints(side,ratio);

	} else if (pointStep === "1"
		&& maxPoints < 99
		&& input.value.length >= len
		&& !containsElement(filter,keyCode)
	) {

		changeFocus(input);
		totalPoints(side,ratio);

	} else if (
		pointStep === ".25"
		&& input.value.length >= 2
		&& !containsElement(filter,keyCode)
	) {

		if (/\.$/.test(input.value)) {
			var number = input.value;
			number = number.slice(0,1);
			number = number * 1;
			number = number + 20.5;
			input.value = number;
		} else if (/5$/.test(input.value)) {
			var number = input.value.slice(0,2);
			number = number * 1;
			number = number / 10;
			number = number + 20;
			input.value = number;
		} else if (/2$/.test(input.value)) {
			var number = input.value.slice(0,2);
			number = number * 1;
			number = number / 10;
			number = number + 20;
			number += .05;
			input.value = number;
		} else if (/7$/.test(input.value)) {
			var number = input.value.slice(0,2);
			number = number * 1;
			number = number / 10;
			number = number + 20;
			number += .05;
			input.value = number;
		} else {
			var number = input.value.slice(0,1);
			number = number * 1;
			number = number + 20;
			input.value = number;
		}

		changeFocus(input);
		totalPoints(side,ratio);

	} else if(
		( input.value.length >= len || keyCode === 9)
		&& !containsElement(filter,keyCode)
		&& input.value != 10
	) {

		if (len == 3) {
			if (/\.$/.test(input.value)) {
				var number = input.value;
				number = number.slice(0,2);
				number = number * 1;
				number += .5;
				input.value = number;
			} else if (/5$/.test(input.value)) {
				input.value = input.value/10;
			} else {
				input.value = input.value.slice(0,2);
			}

		}

		if (len == 2) {

			if (step == 1) {

			} else {

				if (/\.$/.test(input.value)) {

					var number = input.value;
					number = number.slice(0,1);
					number = number * 1;
					number += .5;
					input.value = number;

				} else if (/5$/.test(input.value)) {

					input.value = input.value/10;

				} else if (input.value < 0) {

				} else if (input.value === -5) {

					input.value = -0.5;

				} else if (input.value === "-.") {

					input.value = -0.5;

				} else {

					// input.value = input.value.slice(0,1);

				}
			}

		}

		totalPoints(side,ratio);
		changeFocus(input);
		return;

    }

	function totalPoints(side,ratio) {

		var class_target = "points_" + side;
		var points_array = document.getElementsByClassName(class_target);
		var total = 0;

		for(var i=0; i<points_array.length;i++){
			if (parseFloat(points_array[i].value)) total += parseFloat(points_array[i].value);
		}

		var points_total = document.getElementById(class_target);
		points_total.value = total * ratio;

		var class_target = "ranks_" + side;
		var ranks_array = document.getElementsByClassName(class_target);
		var total = 0;

		for(var i=0;i<ranks_array.length;i++){
			if(parseFloat(ranks_array[i].value)) total += parseFloat(ranks_array[i].value);
		}

		var ranks_total = document.getElementById(class_target);
		ranks_total.value = total;
		return;
	}

    function containsElement(arr, ele) {
        var found = false, index = 0;
        while(!found && index < arr.length)
        if(arr[index] == ele)
        found = true;
        else
        index++;
        return found;
    }

	function changeFocus(input) {
		if (input.value === "") {
			return;
		}
		var next_index = getIndex(input) + 1;
        while(input.form[next_index].tabIndex == -1) {
			next_index = next_index + 1;
			if (next_index > input.form.length) break;
		}
       	input.form[next_index].focus();
	}

    return true;
}

function getIndex(input) {
	var index = -1, i = 0, found = false;
	while (i < input.form.length && index == -1)
	if (input.form[i] == input)index = i;
	else i++;
	return index;
}


/* Autoselect which kids are valid speakers for WSDC style debate ballot entry */

function autoSel(input, event) {

	if ($("#toggleKeyboardShortcuts").prop("checked") === false) {
		return;
	}

    var keyCode = event.keyCode;

	if (keyCode === 9) {
		return;
	}

	var next_index = getIndex(input) + 1;

	while(input.form[next_index].tabIndex == -1) {
		next_index = next_index + 1;
		if (next_index > input.form.length) break;
	}

	input.form[next_index].focus();

}

/* Auto advance to next input when entering ballots */

function autoTab(input,len,e) {

	if ($("#toggleKeyboardShortcuts").prop("checked") === false) {
		return;
	}

    var keyCode = e.keyCode;

    var filter = [0,8,9,16,17,18,37,38,39,40,46];

	if (len == 9 && input.value.length >= 2 && !containsElement(filter,keyCode)) {

        if (input.value != 30) {
			var number = input.value;
			number = number * 1;
			number = number / 10;
			number = number + 20;
			input.value = number;
		}

		changeFocus(input);

	} else if (len == 6 && input.value.length >= 2 && !containsElement(filter,keyCode)) {

		if (/\.$/.test(input.value)) {
			var number = input.value;
			number = number.slice(0,1);
			number = number * 1;
			number = number + 20.5;
			input.value = number;
		} else if (/5$/.test(input.value)) {
			var number = input.value.slice(0,2);
			number = number * 1;
			number = number / 10;
			number = number + 20;
			input.value = number;
		} else if (/2$/.test(input.value)) {
			var number = input.value.slice(0,2);
			number = number * 1;
			number = number / 10;
			number = number + 20;
			number += .05;
			input.value = number;
		} else {
			var number = input.value.slice(0,1);
			number = number * 1;
			number = number + 20;
			input.value = number;
		}

		changeFocus(input);

	} else if(input.value.length >= len && !containsElement(filter,keyCode) && input.value != 10) {

		if (len == 3) {

			if (/\.$/.test(input.value)) {
				var number = input.value;
				number = number.slice(0,2);
				number = number * 1;
				number += .5;
				input.value = number;
			} else if (/5$/.test(input.value)) {
				input.value = input.value/10;
			} else {
				input.value = input.value.slice(0,2);
			}

		}

        if (len == 2 && input.value != 100) {
            input.value = input.value.slice(0, len);
        }

		changeFocus(input);
    }

    function containsElement(arr, ele) {
        var found = false, index = 0;
        while(!found && index < arr.length)
        if(arr[index] == ele)
        found = true;
        else
        index++;
        return found;
    }

	function changeFocus(input) {

		var next_index = getIndex(input) + 1;

        while(input.form[next_index].tabIndex == -1) {
			next_index = next_index + 1;
			if (next_index > input.form.length) break;
		}
       	input.form[next_index].focus();
	}

    return true;
}

/* Login Box and other initializations */


$(document).ready(function() {

	$('a.login-window').click(function() {
		var loginBox = $(this).attr('href');
		$(loginBox).slideDown(300);
		var popMargTop = ($(loginBox).height() + 24) / 2;
		var popMargLeft = ($(loginBox).width() + 24) / 2;
		$(loginBox).css({ 'margin-top' : -popMargTop, 'margin-left' : -popMargLeft });
		$('body').append('<div id="mask"></div>');
		$('#mask').fadeIn(300);
		$('#username').focus();
		return false;
	});

	$('a.close, #mask').live('click', function() {
		$('#mask').fadeOut(300 , function() { $('#mask').remove();  });
		$('.login-popup').slideUp(300);
		return false;
	});

	$('.hide-menu').click(function() {
		$('.menu').slideUp(300);
		$('.content').addClass('nomenu');
		$('.hide-menu').addClass('hidden');
		$('.show-menu').removeClass('hidden');
	});

	$('.show-menu').click(function() {
		$('.menu').slideDown(300);
		$('.content').removeClass('nomenu');
		$('.show-menu').addClass('hidden');
		$('.hide-menu').removeClass('hidden');
	});

	resizeAll();

});

// Resize the inputs and be done with the eighty seven years of choosing I've
// been doing.

var waitForFinalEvent = (function () {
  var timers = {};
  return function (callback, ms, uniqueId) {
    if (!uniqueId) {
      uniqueId = "Don't call this twice without a uniqueId";
    }
    if (timers[uniqueId]) {
      clearTimeout (timers[uniqueId]);
    }
    timers[uniqueId] = setTimeout(callback, ms);
  };
})();

function resizeAll() {

	$('input[type=text], input[type=number].sizeme, input[type=email], input[type=tel], input[type=date], input[type=time], input[type=url]').each(function(){
		if ($(this).is(':visible') && !['TD','TH','LABEL'].includes($(this).parent()[0].tagName)) {
			$(this).width($(this).parent().width()-10);
		}
	});

	$('textarea').each(function(){
		$(this).width($(this).parent().width()-10);
	});

	$('.chosen-container').each(function(){
		if (
			$(this).parent().is("td")
			|| $(this).parent().is("th")
		) {
		} else {
			$(this).width($(this).parent().width()-10);
		}
	});
}

function toggleView(elementId, elementClass) {
	$("."+elementClass).addClass('hidden');
	$("."+elementId).removeClass('hidden');
	fixVisual();
}

$(window).resize(function () {
	waitForFinalEvent(function(){
		resizeAll();
	}, 150, "ThisIsSupposedToBeUniqueTheySay");
});

