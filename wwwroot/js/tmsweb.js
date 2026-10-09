(function ($) {
        $("#frmEdit").ajaxForm(function (data) {
        killOverlayForm();
        if (data.result === true) {
        $.alert.open('info', data.resultMessage, function (r) {
        window.opener.reloadRow();
        window.close();
        });
        } else {
        $.alert.open('error', data.resultMessage);
        }
        });

        $('#supplyer_code').change(function () {
        var supplyer = $(this).val();
        if (supplyer == 'SP26001') {
        $('#truck_code').prop('disabled', false);
        $('#driver_code').prop('disabled', false);
        setDriverDropDown(supplyer);
        } else {
        $('#truck_code').val('');
        $('#driver_code').val('');
        $('#truck_code').prop('disabled', true);
        $('#driver_code').prop('disabled', true);
        }
        });

        $('#truck_code').change(function () {
        var truck = $(this).val();
        setSelectDriver(truck);
        });
        }(jQuery));

        function setTruckDropDown(supplyer) {
        $("#truck_code option").remove();
        $.post("/AutoComplete/GetTruckBySupplyer", {
        code: supplyer
        }, function (data) {
        $('#truck_code').append("<option>-----เลือก-----</option>");
        $.each(data.result, function (i, item) {
        var option = "<option value='" + item.truck_code + "'>" + item.truck_license +
        "</option>"
        $('#truck_code').append(option)
        });
        if (sgfpplyer != 'SP26001') {
        $('#truck_code').append("<option value='T000000'>ไม่ระบุ</option>");
        }
        });
        }

        function setDriverDropDown(supplyer) {
        $("#driver_code option").remove();
        $.post("/Order/GetDriverData", {

        }, function (data) {
        $('#driver_code').append("<option>-----เลือก-----</option>");
        $.each(data.result, function (i, item) {
        var option = "<option value='" + item.driver_code + "'>" + item.fullname +
        "</option>"
        $('#driver_code').append(option)
        });
        if (supplyer != 'SP26001') {
        $('#driver_code').append("<option value='DV000000'>ไม่ระบุ</option>");
        }
        });
        }

        function setSelectDriver(truck) {
        $.post("/Order/GetTruckDriverByTruckCode", {
        code: truck
        }, function (result) {
        $('#driver_code').val(result.driver_code);
        });
        }

        function OnValidate() {
        var result = 0;
        var e = $('#driver_cost');
        var supplier = $('#supplyer_code').val();
        var truck = $('#truck_code').val();
        var driver = $('#driver_code').val();
        var truck_element = $('#truck_code');
        var driver_element = $('#driver_code');

        if (supplier == 'SP26001') {
        if (truck === '') {

        truck_element.closest("div.form-group").addClass("has-error");
        truck_element.closest('div.form-group').find('span.help-block').html('กรุณาเลือกทะเบียนรถ');
        result++;
        } else {
        truck_element.closest("div.form-group").removeClass("has-error");
        truck_element.closest('div.form-group').find('span.help-block').html('');
        }

        if (driver === '') {

        driver_element.closest("div.form-group").addClass("has-error");
        driver_element.closest('div.form-group').find('span.help-block').html('กรุณาเลือกทะเบียนรถ');
        result++;
        } else {
        driver_element.closest("div.form-group").removeClass("has-error");
        driver_element.closest('div.form-group').find('span.help-block').html('');
        }
        } else {
        truck_element.closest("div.form-group").removeClass("has-error");
        truck_element.closest('div.form-group').find('span.help-block').html('');

        driver_element.closest("div.form-group").removeClass("has-error");
        driver_element.closest('div.form-group').find('span.help-block').html('');
        }

        if (result === 0) {
        overLayForm();
        return true;
        } else {
        return false;
        }
        }