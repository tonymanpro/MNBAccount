/*******prototype*********/
String.prototype.padLeft = function (n, s) {
    var t = this, L = s.length;
    while (t.length + L <= n) t = s + t;
    if (t.length < n) t = s.substring(0, n - t) + t;
    return t;
}
String.prototype.padRight = function (n, s) {
    var t = this, L = s.length;
    while (t.length + L <= n) t += s;
    if (t.length < n) t += s.substring(0, n - t);
    return t;
}
/*******prototype*********/
$monibyte = {
    parseBool: function (value) {
        if (value) {
            return eval(value.toLowerCase());
        }
        return false;
    },
    parseDate: function (value) {
        return kendo.parseDate(value);
    },
    parseFloat: function (value) {
        return kendo.parseFloat(value);
    },
    parseInt: function (value) {
        return kendo.parseInt(value);
    },
    formatNumber: function (value, settings) {
        var options = jQuery.extend({
            float: true,
            group: true,
        }, settings);
        var result = kendo.toString(value, options.float ? "n" : "n0");
        if (!options.group) {
            if (result) {
                result = result.replace(kendo.culture().numberFormat[","], "");
            }
        }
        return result;
    },
    formatDate: function (value, settings) {
        var options = jQuery.extend({
            format: "d"
        }, settings);
        return kendo.toString(value ? value : "", options.format);
    },
    format: function (str, args) {
        args = typeof args === 'object' ? args : Array.prototype.slice.call(arguments, 1);
        return str.replace(/\{\{|\}\}|\{(\w+)\}/g, function (m, n) {
            if (m == "{{") { return "{"; }
            if (m == "}}") { return "}"; }
            return args[n];
        });
    },
    print: function () {
        try {
            document.execCommand('print', false, null);
        }
        catch (e) {
            window.print();
        }
    },
    daterange: function (idfrom, idto) {
        $(idto).rules('add', { daterange: idfrom });
    },
    clearFormElements: function (ele) {
        $(ele).find(':input').each(function () {
            switch (this.type) {
                case 'hidden':
                case 'password':
                case 'select-multiple':
                case 'select-one':
                case 'text':
                case 'textarea':
                    $(this).val('');
                    break;
                case 'checkbox':
                case 'radio':
                    this.checked = false;
            }
            var zeroi = $monibyte.formatNumber(0);
            var zerof = $monibyte.formatNumber(0, { float: true });
            $('.mb-numeric-int').val(zeroi);
            $('.mb-numeric-float').val(zerof);
        });
    },
    blockPage: function () {
        $.blockUI({
            message: $('div.op-espera'),
            css: { left: '48%', padding: '5px' }
        });
    },
    unblockPage: function () {
        $.unblockUI();
    },
    blockRedirect: function (url) {
        setTimeout(function () {
            $monibyte.blockPage();
            window.location = url;
        }, 0);
    },
    executeAjax: function (settings) {
        var _defaults = {
            name: "executeAjax",
            httpMethod: "POST",
            async: true,
            actionUrl: null,
            jsonData: null,
            callback: null,
            operation: "change",
            contentType: "application/json; charset=utf-8"
        };
        var options = jQuery.extend({}, _defaults, settings);
        //cambiar nombres de funciones a funciones reales
        if (!jQuery.isFunction(options.dataFunc)) {
            options.dataFunc = window[options.dataFunc];
        }
        if (!jQuery.isFunction(options.callback)) {
            options.callback = window[options.callback];
        }
        //obtener los datos requeridos para el ajax
        if (jQuery.isFunction(options.dataFunc)) {
            options.jsonData = options.dataFunc(options.jsonData);
        }
        var submitAjax = function (_settings) {
            if (_settings.contentType == _defaults.contentType) {
                _settings.jsonData = JSON.stringify(_settings.jsonData);
            }
            $.ajaxSetup({ 'async': _settings.async });
            $.ajax({
                contentType: _settings.contentType,
                type: _settings.httpMethod,
                url: _settings.actionUrl,
                data: _settings.jsonData,
                success: function (_data) {
                    var _callbackData = _data;
                    if (_settings.targetId) {
                        _callbackData = _settings.callbackData;
                        if (_data.length > 0) {
                            var targetId = _settings.targetId;
                            var selector = $.type(targetId) == "string" ?
                                document.getElementById(targetId) : targetId;
                            if (_settings.operation == "change") {
                                $(selector).html(_data);
                            } else if (_settings.operation == "prepend") {
                                $(selector).prepend(_data);
                            } else if (_settings.operation == "append") {
                                $(selector).append(_data);
                            } else if (_settings.operation == "replace") {
                                $(selector).replaceWith(_data);
                            }
                        }
                    }
                    if (jQuery.isFunction(_settings.callback)) {
                        _settings.callback(_callbackData);
                    }
                }
            });
            $.ajaxSetup({ 'async': true });
        };
        if (options.confirm) {
            $.jConfirm(options.confirmMsg, "highlight", function (conf) {
                if (conf) {
                    submitAjax(options);
                }
            }, options);
        } else {
            submitAjax(options);
        }
    },
    startTimeOut: function (settings) {
        var options = jQuery.extend({
            time: 0,
            done: null,
        }, settings);
        var timeout;
        var startup = function (_options) {
            setTimeout(function () {
                timeout = setTimeout(_options.done, _options.time);
            }, 0);
        };
        startup(options);
        return {
            cancel: function () {
                clearTimeout(timeout);
            },
            restart: function () {
                clearTimeout(timeout);
                startup(options);
            }
        }
    }
}
jQuery.fn.serializeFormJSON = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};

jQuery.fn.exists = function () { return this.length > 0; };

/**Control de eventos de ajax**/
$(document).ajaxStart(function (event, request, settings) {
    $monibyte.blockPage();
});
$(document).ajaxStop(function (event, request, settings) {
    $monibyte.unblockPage();
    if (jQuery.isFunction(window.onAjaxStop)) {
        window.onAjaxStop();
    }
});
$(document).ajaxError(function (event, request, settings) {
    event.preventDefault();
    try {
        if (request.responseText) {
            var responsetext = request.responseText;
            var responsejson = $.parseJSON(responsetext);
            var callback = request.status == 403 ? function () {
                $monibyte.blockRedirect(window.location.href);
            } : null;
            if (jQuery.isFunction(window.onAjaxError)) {
                window.onAjaxError(responsejson, callback);
            }
        }
    } catch (err) {
        console.error(err);
    }
});

var notify = null;
$(function () {
    notify = $("<div/>").kendoNotification()
        .data("kendoNotification");
});
function mostrarResultadoExitoso() {
    notify.setOptions({
        show: function (e) {
            e.element.parent().css({
                zIndex: 20000
            });
        },
        position: { pinned: true, right: 20, top: 207, left: null }
    });
    notify.show($(".op-exitoso").html(), "success");
}
/**Control de eventos de ajax**/