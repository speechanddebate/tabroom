/* confirmation dialog */

	function confirmAction(link, message, dest, payload) { 

		alertify.confirm(
			"Are you sure?",
			message,
			function(event) {
				if (event) { 
                    if (typeof(link)==='function' 
						&& typeof(dest)!='undef' 
						&& typeof(payload)!='undef'
					){
                        link(dest,payload);
                    }else{
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

/* master toggle for checkboxes */

	function confirmAll(master, targetClass) { 

		if ($(master).prop("checked") === false) { 
			$("."+targetClass).prop("checked", false);
		} else { 
			$("."+targetClass).prop("checked", true);
		}
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

		alertify.confirm(alertMessage, function(e) { 
	
			if (e) { 
				postSwitch(checkObject, replyUrl);
				return;
			} else {
				return;
			}

		});

		return;
	}

	function postSwitch(checkObject, replyUrl, callback) { 

		var targetId      = $(checkObject).attr("target_id");
		var propertyName  = $(checkObject).attr("property_name");
		var settingName   = $(checkObject).attr("setting_name");

		var successAction = $(checkObject).attr("on_success");
		var replyTarget   = $(checkObject).attr("reply_target");
		var replyAppend   = $(checkObject).attr("reply_append");
		var newParent     = $(checkObject).attr("new_parent");

		var propertyValue = checkObject.value;

		var otherObject = $(checkObject).attr("other_value");
		var otherTextId = $(checkObject).attr("other_text");

		var parentObject = $(checkObject).parent();
		var parentObjectId = 0;

		if (parentObject) { 
			parentObjectId = parentObject.attr('id');
		}

		var otherValue;

		if (otherObject) { 
			otherValue = $("#"+otherObject).val();
			$("#"+otherObject).val("");
		}

		var otherText;
		if (otherTextId) { 
			otherText = $("#"+otherTextId).val();
		}

		if (propertyValue === undefined) { 
			propertyValue = $(checkObject).attr("value");
		}

		if (propertyValue === undefined) { 
			propertyValue = $(checkObject).val();
		}

		if (checkObject.type === "checkbox") { 
			if ($(checkObject).prop("checked") === false) { 
				propertyValue = 0;
			}
		}

		$.ajax({ 
			type : 'POST',
			url  : replyUrl,
			data : {
				target_id      : targetId,
				property_name  : propertyName,
				setting_name   : settingName,
				property_value : propertyValue,
				other_value    : otherValue,
				other_text     : otherText,
				parent_id      : parentObjectId
			},
			success : function(data) {

				if (data.reply) { 
					
					if (replyTarget) { 
						$("#"+replyTarget).text(data.reply);
					}

					if (replyAppend) { 
						$("#"+replyAppend).append(data.reply);
					}

					$(".replybucket").text(data.reply);
					$(".replyappend").append(data.reply);

				}

				if (data.error) { 

					alertify.error(data.message);
					
				} else if (data.message) { 

					alertify.dismissAll();
					alertify.notify(data.message, "custom");

					if (data.destroy) { 
						$("#"+data.destroy).remove();
					}

					if (successAction === "destroy") { 
						$("#"+targetId).remove();
					} else if (successAction === "hide") { 
						$("#"+targetId).addClass("hidden");
					} else if (
						successAction === "refresh" 
						|| successAction === "reload"
						|| data.refresh
					) { 
						window.location.reload();
					}

					if (newParent) { 
						$("#"+targetId).prependTo("#"+newParent);
					}

					if (data.newParent) { 
						$("#"+targetId).prependTo("#"+data.newParent);
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

					if (data.norefresh) { 

					} else { 
						$('table').trigger('applyWidgets');
						$('table').trigger('update', [true]);
					}

				} else { 

					console.log(data);
					alertify.warning("An error condition was tripped.");
				}

				if (callback) { 
					callback(data);
				}

				return;
			}
		});
	}

/* zebra stripe the rows */ 

	function zebraRows() {

		$(".main").find(".row:even").addClass("even");
		$(".main").find(".row:odd").addClass("odd");

		$(".blankfull").find(".row:even").addClass("even");
		$(".blankfull").find(".row:odd").addClass("odd");

		$(".menu").find(".row:even").addClass("even");
		$(".menu").find(".row:odd").addClass("odd");

		$(".sidenote").find(".row:even").removeClass("odd");
		$(".sidenote").find(".row:odd").removeClass("even");
		$(".sidenote").find(".row:even").addClass("even");
		$(".sidenote").find(".row:odd").addClass("odd");

		$("table").find(".row:even").removeClass("odd");
		$("table").find(".row:even").addClass("even");

		$("table").find(".row:odd").removeClass("even");
		$("table").find(".row:odd").addClass("odd");

		$(".pagehalf").find(".row:even").addClass("even");
		$(".pagehalf").find(".row:even").removeClass("odd");
		$(".pagehalf").find(".row:odd").addClass("odd");
		$(".pagehalf").find(".row:odd").removeClass("even");

		$(".main").find(".lightrow:even").addClass("lighteven");
		$(".main").find(".lightrow:even").removeClass("lightodd");
		$(".main").find(".lightrow:odd").addClass("lightodd");
		$(".main").find(".lightrow:odd").removeClass("lighteven");

		$(".blankfull").find(".lightrow:even").addClass("lighteven");
		$(".blankfull").find(".lightrow:even").removeClass("lightodd");
		$(".blankfull").find(".lightrow:odd").addClass("lightodd");
		$(".blankfull").find(".lightrow:odd").removeClass("lighteven");

		$(".menu").find(".lightrow:even").addClass("lighteven");
		$(".menu").find(".lightrow:odd").addClass("lightodd");

		$(".sidenote").find(".lightrow:even").removeClass("lightodd");
		$(".sidenote").find(".lightrow:even").addClass("lighteven");
		$(".sidenote").find(".lightrow:odd").removeClass("lighteven");
		$(".sidenote").find(".lightrow:odd").addClass("lightodd");

		$("table").find(".lightrow:even").removeClass("lightodd");
		$("table").find(".lightrow:even").addClass("lighteven");
		$("table").find(".lightrow:odd").removeClass("lighteven");
		$("table").find(".lightrow:odd").addClass("lightodd");

		$(".pagehalf").find(".lightrow:even").addClass("lighteven");
		$(".pagehalf").find(".lightrow:even").removeClass("lightodd");

		$(".pagehalf").find(".lightrow:odd").addClass("lightodd");
		$(".pagehalf").find(".lightrow:odd").removeClass("lighteven");
	};


/* Change the file uploader div to show the name of the uploaded file */

function uploaderName(uploader, filedisplay) { 

	if (uploader == null) { uploader = 'upload'; }
	if (filedisplay == null) { filedisplay = 'filename'; }

	var filename = document.getElementById(uploader).value;
	var lastIndex = filename.lastIndexOf("\\");
	if (lastIndex >= 0) {
		filename = filename.substring(lastIndex + 1);
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

/* Login Box */

$(document).ready(function() { $('a.login-window').click(function() { 
	var loginBox = $(this).attr('href'); 
	$(loginBox).slideDown(300); 
	var popMargTop = ($(loginBox).height() + 24) / 2; 
	var popMargLeft = ($(loginBox).width() + 24) / 2;
	$(loginBox).css({ 'margin-top' : -popMargTop, 'margin-left' : -popMargLeft }); 
	$('body').append('<div id="mask"></div>'); 
	$('#mask').fadeIn(300); return false; }); 
	
	$('a.close, #mask').live('click', function() { 
		$('#mask').fadeOut(300 , function() { $('#mask').remove();  }); 
		$('.login-popup').slideUp(300); 
		return false; 
	}); 

});

$(document).ready(function() { $('.hide-menu').click(function() { 
		$('.menu').slideUp(300); 
		$('.content').addClass('nomenu');
		$('.hide-menu').addClass('hidden');
		$('.show-menu').removeClass('hidden');
	});
});

$(document).ready(function() { $('.show-menu').click(function() { 
		$('.menu').slideDown(300); 
		$('.content').removeClass('nomenu');
		$('.show-menu').addClass('hidden');
		$('.hide-menu').removeClass('hidden');
	});
});


