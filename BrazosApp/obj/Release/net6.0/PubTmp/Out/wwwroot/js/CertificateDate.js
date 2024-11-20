function DateModifier(date) {
    date = date.replace(/\//g, '-');
    console.log(date);
    var dt = date.split(' ')
    var s = dt[0].split('-');

    var month = parseInt(s[1]) - 1;
    console.log(s[2]);
    console.log(month);
    console.log(s[0]);


    $("#certexpdT").datepicker("setDate", new Date(s[2], month, s[0]));
    console.log($("#certexpdT").val())
    //$('#CertExpDate').val(new Date($(certexpdT).val()));
    $('#CertExpDate').val(($(certexpdT).val()));
}

//function DateModifier(date) {
//    date = date.replace(/\//g, '-');
//    console.log(date);
//    var dt = date.split(' ')
//    var s = dt[0].split('-');

//    var month = parseInt(s[0]) - 1;
//    console.log(s[2]);
//    console.log(month);
//    console.log(s[0]);


//    $("#certexpdT").datepicker("setDate", new Date(s[2], month, s[1]));
//    console.log($("#certexpdT").val())
//    $('#CertExpDate').val(new Date($(certexpdT).val()));
//}