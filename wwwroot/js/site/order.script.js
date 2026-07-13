(function ($) {
    $("#frmAddContainer").ajaxForm(function (data) {
        killOverlayForm();
        if (data.result === true) {
            $.alert.open('info', data.resultMessage, function (r) {
                window.location.reload();
            });
        } else {
            $.alert.open('error', data.resultMessage);
        }
    });

    $('#txtCostCode').typeahead({
        minLength: 1,
        source: function (query, process) {
            var list = [];
            map = {};
            return $.post('/AutoComplete/TransportRate', { term: query }, function (data) {
                $.each(data, function (i, item) {
                    map[item.title] = item;
                    list.push(item.title);
                });
                process(list);
            });
        },
        updater: function (item) {
            var code = map[item].value;
            setCustomerBox(code, 'cont');
            return item;
        }
    });

    $('#txtStartCode').typeahead({
        minLength: 1,
        source: function (query, process) {
            var list = [];
            map = {};
            return $.post('/AutoComplete/PlaceByType', { term: query, action: "S", jobtype: $('.rdoTradeType:checked').val() }, function (data) {
                $.each(data, function (i, item) {
                    map[item.title] = item;
                    list.push(item.title);
                });
                process(list);
            });
        },
        updater: function (item) {
            var code = map[item].value;
            $('#valueChange').val('S');
            setYardPortBox(code, 'cont');
            return item;
        }
    });

    $('#txtFinishCode').typeahead({
        minLength: 1,
        source: function (query, process) {
            var list = [];
            map = {};
            return $.post('/AutoComplete/PlaceByType', { term: query, action: "F", jobtype: $('.rdoTradeType:checked').val() }, function (data) {
                $.each(data, function (i, item) {
                    map[item.title] = item;
                    list.push(item.title);
                });
                process(list);
            });
        },
        updater: function (item) {
            var code = map[item].value;
            $('#valueChange').val('F');
            setYardPortBox(code, 'cont');
            return item;
        }
    });

    $('#container_kind').change(function () {
        var val = $(this).val();
        if (val == '99') {
            $('#txtOther').prop('disabled', false);
            $('#txtOther').focus();
        }
        else {
            $('#txtOther').val('');
            $('#txtOther').prop('disabled', true);
        }
    });

    $('.rdoTradeType').change(function () {
        var x = $(this).val();
        $('#txtStartCode').val('');
        $('#txtDisStartName').val('');
        $('#txtFinishCode').val('');
        $('#txtDisFinishName').val('');
    });
}(jQuery));

function searchYardPortFunc() {
    var url = '';
    var setting = 'width=900,height=550';
    url = '/search/yardport?ev=flr';

    window.open(url, 'search place', setting);
}

function searchPlaceFunc(action) {
    var url = '';
    var setting = 'width=900,height=550';
    var type = $('.rdoTradeType:checked').val();
    $('#valueChange').val(action)

    if (action == 'S') {
        if (type == "IMPORT") {
            url = '/search/port';
        }
        else if (type == "EXPORT") {
            url = '/search/yard';
        }
    }
    else if (action == 'F') {
        if (type == "IMPORT") {
            url = '/search/yard';
        }
        else if (type == "EXPORT") {
            url = '/search/port';
        }
    }
    else {
        url = '/search/place'
    }

    window.open(url, 'search place', setting);
}

function searchCustomerFunc(e) {
    var url = '../search/transportrate?ev=' + e;
    var setting = 'width=900,height=550';
    window.open(url, 'search customer', setting);
}

function setYardPortBox(code, type) {
    var event = $('#job_type_handle').val();
    var action = $('#valueChange').val();
    overLayForm();
    $.post("../Order/GetYardPort",
        {
            code: code
        }, function (result) {
            var code = result.place_code;
            var name = result.place_name;
            var type = result.place_type;
            if (event == 'flr') {
                $('#txtFloorStartCode').val(code);
                $('#txtFloorDisStartName').val(name);
                $('#txtStartType').val(type);
            }
            else if (event == 'cont') {
                if (action === 'S') {
                    $('#txtStartCode').val(code);
                    $('#txtDisStartName').val(name);
                }
                else if (action === 'F') {
                    $('#txtFinishCode').val(code);
                    $('#txtDisFinishName').val(name);
                }
            }
            else if (event == 'dom') {
                if (action === 'S') {
                    $('#txtDomesticStartCode').val(code);
                    $('#txtDomesticStartName').val(name);
                }
                else if (action === 'F') {
                    $('#txtFinishCode').val(code);
                    $('#txtDisFinishName').val(name);
                }
            }
            else {
                if (action === 'S') {
                    $('#txtOceanStartCode').val(code);
                    $('#txtOceanDisStartName').val(name);
                }
                else if (action === 'F') {
                    $('#txtOceanFinishCode').val(code);
                    $('#txtOceanDisFinishName').val(name);
                }
            }
            killOverlayForm();
        });
}

function setCustomerBox(code, event) {
    overLayForm();
    $.post("../Order/GetCustomerCost",
        {
            code: code
        }, function (result) {
            if (event == 'cont') {
                $('#txtCostCode').val(result.cost_code);
                $('#txtCustomerCode').val(result.customer_code);
                $('#txtCustomerName').val(result.customer_name_th);
                $('#txtMissionCode').val(result.mission_code);
                $('#txtExpressway').val(result.expressway_value);
                $('#txtDisMissionName').val(result.mission_name);
                $('#acronym').val(result.customer_acronym);
            }
            else if (event == 'flr') {
                $('#txtFloorCostCode').val(result.cost_code);
                $('#txtFloorCustomerCode').val(result.customer_code);
                $('#txtFloorCustomerName').val(result.customer_name_th);
                $('#txtFloorMissionCode').val(result.mission_code);
                $('#txtFloorDisMissionName').val(result.mission_name);
                $('#txtFloorExpressway').val(result.expressway_value);
                $('#txtMissionType').val(result.mission_type);
                $('#flooracronym').val(result.customer_acronym);
            }
            else if (event == 'dom') {
                $('#txtDomesticCostCode').val(result.cost_code);
                $('#txtDomesticCustomerCode').val(result.customer_code);
                $('#txtDomesticCustomerName').val(result.customer_name_th);
                $('#domesticacronym').val(result.customer_acronym);
                $('#route_code').val(result.mission_code);
                $('#txtDomesticRouteName').html(result.mission_name);
                $('#txtDomExpressway').val(result.expressway_value);
            }
            else {
                $('#txtOceanCostCode').val(result.cost_code);
                $('#txtOceanCustomerCode').val(result.customer_code);
                $('#txtOceanCustomerName').val(result.customer_name_th);
                $('#txtOceanMissionCode').val(result.mission_code);
                $('#txtOceanDisMissionName').val(result.mission_name);
                $('#txtOceanExpressway').val(result.expressway_value);
                $('#oceanacronym').val(result.customer_acronym);
            }

            killOverlayForm();
        });
    $('#txtSearchCustomer').val('');
}

function OnValidate() {
    var fn = false;
    fn = $('#frmAdd').myfunction({
        rules: {
            txtCostCode: {
                text_button: {
                    message: "กรุณาพิมพ์รหัสลูกค้า"
                }
            },
            txtStartCode: {
                text_button: {
                    message: "กรุณาพิมพ์รหัสสถานที่รับตู้"
                }
            },
            container_kind: {
                selected: {
                    message: "กรุณาเลือกข้อมูล"
                }
            },
            txtJobDate: {
                require: {
                    value: true,
                    message: "กรุณาพิมพ์วันที่เริ่มงาน"
                }
            },
            txtClosing: {
                require: {
                    value: true,
                    message: "กรุณาพิมพ์วันที่ปิดรับ-ส่งสินค้า"
                }
            },
            txtInvoice: {
                require: {
                    value: true,
                    message: "กรุณาพิมพ์เลขที่ Invoice/Job No"
                }
            },
            txtAmount: {
                require: {
                    value: true,
                    message: "กรุณาพิมพ์จำนวนตู้"
                }
            },
            txtBookingNo: {
                require: {
                    value: true,
                    message: "กรุณาพิมพ์เลขที่ Booking"
                }
            },
            agent_no: {
                selected: {
                    message: "กรุณาเลือกข้อมูล"
                }
            },
            truck_type: {
                selected: {
                    message: "กรุณาเลือกข้อมูล"
                }
            },
        }
    })

    if (fn)
        overLayForm();

    return fn;
}