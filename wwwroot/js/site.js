(function ($) {
    $("#frmFinish").ajaxForm(function (data) {
        killOverlayForm();
        if (data.result) {
            $.alert.open('info', data.resultMessage, function (r) {
                window.opener.reloadRow();
                window.close();
            });
        } else {
            $.alert.open('error', data.resultMessage);
        }
    });

    $(document.body).delegate('.cashing-val', 'change', function () {
        var cost = 0;
        var x = $(this).val();
        var arr = x.split('+');
        $.each(arr, function (index, value) {
            if (value != '') {
                cost = cost + parseFloat(value);
            }
        });

        if (cost > 0) {
            $(this).val(cost);
        }
        TotalCashingSum()
    });

    $(document.body).delegate('.advance-val', 'change', function () {
        var cost = 0;
        var x = $(this).val();
        var arr = x.split('+');
        $.each(arr, function (index, value) {
            if (value != '') {
                cost = cost + parseFloat(value);
            }
        });

        if (cost > 0) {
            $(this).val(cost);
        }
        TotalAdvanceSum()
    });

    $('.form-box').keyup(function () {

    });

}(jQuery));

function addCashing() {
    var row = '<tr><td style="text-align:center;vertical-align:middle;"><a href="#" class="closed" onclick=removeCashingItem(this)><i class="icon icon-close"></a></td>'
    row += '<td><input type="text" class="form-control form-box" name="nameCashing[] autocomplete="Off"></td>';
    row += '<td><input type="text" class="form-control cashing-val form-box" name="valWithdrawCashing[]" value="0" autocomplete="Off"/></td>'
    row += '<td><input type="text" class="form-control cashing-val form-box" name="valCashing[]" value="0" autocomplete="Off"/></td></tr>'
    $('#cashing-table').append(row);
}

function addAdvance() {
    var row = $('#advance-table tr td').html();
    if (row == "No matching records found") {
        $('#advance-table tr').remove();
    }
    var row = '<tr><td style="text-align:center;vertical-align:middle;"><a href="#" class="closed" onclick=removeAdvanceItem(this)><i class="icon icon-close"></a></td>'
    row += '<td><input type="text" class="form-control form-box" name="nameAdvance[]" autocomplete="Off"></td>';
    row += '<td><input type="text" class="form-control other-val form-box" name="valWithdrawAdvance[]" value="0" autocomplete="Off"/></td>'
    row += '<td><input type="text" class="form-control other-val form-box" name="valAdvance[]" value="0" autocomplete="Off"/></td></tr>'
    $('#advance-table').append(row)
}

function removeCashingItem(e) {
    //var val = $(e).closest("td").next().next().find("input").val();
    $(e).parent().parent().remove()
    TotalCashingSum();
}

function removeAdvanceItem(e) {
    //var val = $(e).closest("td").next().next().find("input").val();
    $(e).parent().parent().remove()
    TotalAdvanceSum();
}

function TotalCashingSum() {
    var sum = 0;
    $(".cashing-val").each(function () {
        sum += parseFloat(this.value) || 0;
    });

    $('#total-cashing').html(number_format(sum));
    $('#hidden-total-cashing').val(sum);
}

function TotalAdvanceSum() {
    var sum = 0;
    $(".advance-val").each(function () {
        sum += parseFloat(this.value) || 0;
    });

    $('#total-advance').html(number_format(sum));
    $('#hidden-total-advance').val(sum);
}

function OnValidate() {
    $(".form-box").each(function () {
        if ($(this).val() == '') {
            $(this).val('0');
        }
    });

    $(".nameAdvance").each(function () {
        if ($(this).val() == '') {
            $(this).val('-');
        }
    });

    overLayForm();
    return true;
}

function number_format(number) {
    var decimals = 2;
    var dec_point = '.';
    var thousands_sep = ',';
    number = number.toFixed(decimals);
    var nstr = number.toString();
    nstr += '';
    x = nstr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? dec_point + x[1] : '';
    var rgx = /(\d+)(\d{3})/;

    while (rgx.test(x1))
        x1 = x1.replace(rgx, '$1' + thousands_sep + '$2');

    return x1 + x2;
}