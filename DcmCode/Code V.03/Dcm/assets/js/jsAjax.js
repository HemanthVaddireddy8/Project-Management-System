/*** Start - AJAX ***===============================================================================================*/
var Status = { ERROR: "danger", SUCCESS: "success" };
var ajaxType = { GET: "GET", POST: "POST" };
var ajaxDataType = { JSON: "json", HTML: "html" };
var ajaxAsync = { TRUE: true, FALSE: false };
var ajaxShowProgress = { TRUE: true, FALSE: false };
var ajaxRefresh = { TRUE: true, FALSE: false };


function getAjaxByModel(type, model, url, dataType, isAsync, isShowProgress, fnCallback, spinnerElementId) {

    var value;
    if (type == ajaxType.GET && dataType == ajaxDataType.JSON) {
        $.ajaxSetup({
            async: isAsync,
            dataType: dataType,
            beforeSend: function () {
                if (isShowProgress) {
                    if (spinnerElementId == '') {
                        $("#spinner").fadeIn("fast");
                    }
                    else {
                        $("#" + spinnerElementId).fadeIn("fast");
                    }
                }
            }
        });

        $.getJSON(url, model, function (result) {

        })
            .done(function (result) {
                value = result;
            })
            .fail(function (result) {
                //value = result;

            })
            .always(function () {
                //JSON  Kontroller
                if (value == undefined) {
                    return;
                }
                
               
                if (fnCallback != undefined) {
                    fnCallback(value, isShowProgress, spinnerElementId);
                }
               
            });
    }
    else {
        $.ajax({
            type: type,
            contentType: "application/json; charset=utf-8",
            cache: false,
            url: url,
            dataType: dataType,
            data: JSON.stringify(model),
            async: isAsync,
            beforeSend: function () {
                if (isShowProgress) {
                    if (spinnerElementId == '') {
                        $("#spinner").fadeIn("fast");
                    }
                    else {
                        $("#" + spinnerElementId).fadeIn("fast");
                    }
                }
            },
            success: function (result) {
                value = result;
            },
            complete: function () {
                //JSON  Kontroller
                if (value == undefined) {
                    return;
                }
                if (fnCallback != undefined) {
                    fnCallback(value, isShowProgress, spinnerElementId);
                }

            },
            error: function (result) {
                $('.spinner').hide();
                $('#spinner').fadeOut("fast");;
                console.log("hata oluştu : " + url);
            }
        });

    }
    return value;
}

function getAjax(type, url, dataType, isAsync, isShowProgress, fnCallback, spinnerElementId) {
    var value;
    if (type == ajaxType.GET && dataType == ajaxDataType.JSON) {
        $.ajaxSetup({
            async: isAsync,
            dataType: dataType,
            beforeSend: function () {
                if (isShowProgress) {
                    if (spinnerElementId == '') {
                        $("#spinner").fadeIn("fast");
                    }
                    else {
                        $("#" + spinnerElementId).fadeIn("fast");
                    }
                }
            }
        });

        $.getJSON(url, function (result) {
            //console.log("success");
        })
            .done(function (result) {
                value = result;
            })
            .fail(function () {
                //console.log("error");
            })
            .always(function () {
                //JSON  Kontroller
                if (value == undefined) {
                    return;
                }
               
                if (fnCallback != undefined) {
                    fnCallback(value, isShowProgress, spinnerElementId);
                }
               
            });
    }
    else {
        $.ajax({
            type: type,
            contentType: "application/json; charset=utf-8",
            cache: false,
            url: url,
            dataType: dataType,
            async: isAsync,
            beforeSend: function () {
                if (isShowProgress) {
                    if (spinnerElementId == '') {
                        $("#spinner").fadeIn("fast");
                    }
                    else {
                        $("#" + spinnerElementId).fadeIn("fast");
                    }
                }
            },
            success: function (result) {
                value = result;
            },
            complete: function () {
                //JSON  Kontroller
                if (value == undefined) {
                    return;
                }
                
                if (fnCallback != undefined) {
                    fnCallback(value, isShowProgress, spinnerElementId);
                }
            },
            error: function (result) {
                $('.spinner').hide();
                $('#spinner').fadeOut("fast");;
                console.log("hata oluştu : " + url);
            }
        });
    }
    return value;
}

/*** End - AJAX ***===============================================================================================*/