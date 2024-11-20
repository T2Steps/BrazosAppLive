var paymentDataTable;

function loadPaymentTable(Role) {
    //alert($('#eid').val());
    var encryptedid = $('#encryptedEstId').val();
    var url = window.location.href;
    //alert(id);
    paymentDataTable = $('#PaymentTable').DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "searching": false,
        "bProcessing": true,
        //"paging": false,
        "ajax": {
            "url": "/GetAllPayment?id=" + encryptedid
        },
        "columns": [
            {
                data: 'encryptedId',
                render: function (data, type, row, meta) {
                    return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                }
                , "width": "5%", className: "text-left"
            },
            { "data": "invoiceNo", "width": "10%" },
            { "data": "invoiceDate", "width": "10%" },
            {
                "data": "amount", width: "10%", 'render': function (amount) {
                    if (amount >= 0) {
                        return '$' + amount.toFixed(2);
                    }
                    else if (amount < 0) {
                        return '-$' + Math.abs(amount).toFixed(2);
                    }
                }, className: "text-left"
            },
            { "data": "dueDays", "width": "10%" },
            //{ "data": "paymentDescription[0]", "width": "10%" },
            {
                  "data": function (row) {
                        return Array.isArray(row.paymentDescription) && row.paymentDescription.length > 0
                              ? row.paymentDescription.join(", ")
                              : "--";
                  },
                  "width": "10%"
            },
            { "data": "modeofPay", "width": "10%" },
            { "data": "method", "width": "10%" },
            { "data": "status", "width": "15%" },
            {
                  "data": "encryptedId", "render": function (data, type, row, meta) 
                  {
                        if (!url.includes('View')) {
                              //if (row.status != "Cancelled") {
                              //      return `<a id="lnk_${meta.row}" role="button" href="/GetInvoicePdf?id=${data}" target="blank">
                              //                  <i class="fas fa-file-pdf ml-4" style="color:#022E5F; cursor:pointer" title="View Doc"></i>
                              //            </a>`;
                              //}
                              //else {
                              //      return null
                              //}
                            return `<a id="lnk_${meta.row}" role="button" href="/GetInvoicePdf?id=${data}" target="blank">
                                                <i class="fas fa-file-pdf ml-4" style="color:#022E5F; cursor:pointer" title="View Doc"></i>
                                          </a>`;
                        }
                        else {
                            return `<a id="lnk_${meta.row}" role="button" href="/GetInvoicePdf?id=${data}" target="blank">
                                                <i class="fas fa-file-pdf ml-4" style="color:#022E5F; cursor:pointer" title="View Doc"></i>
                                          </a>`;
                        }
                        //if (row.status == "Cancelled") {
                        //      return `<a id="lnk_${meta.row}" role="button" href="/GetInvoicePdf?id=${data}" target="blank">
                        //                        <i class="fas fa-file-pdf ml-4" style="color:#022E5F; cursor:pointer" title="View Doc"></i>
                        //                  </a>`;
                        //}
                        //else {
                        //      return null
                        //}
 
                  }, "width": "5%"  
            },
            {
                  "data": "encryptedId", "render": function (data, type, row, meta) {
                    if (!url.includes('View')) {
                        if (row.status == "Paid" || row.status == "Refunded") {
                            return `<a id="lnk_${meta.row}" role="button" href="/GetReceiptPdf?id=${data}" target="blank">
                                          <i class="fas fa-file-pdf ml-4" style="color:#022E5F; cursor:pointer" title="View Doc"></i>
                                    </a>`;
                        }
                        else {
                            return null;
                        }
                    }
                    else {
                        if (row.status == "Paid" || row.status == "Refunded") {
                            return `<a id="lnk_${meta.row}" role="button" href="/GetReceiptPdf?id=${data}" target="blank">
                                          <i class="fas fa-file-pdf ml-4" style="color:#022E5F; cursor:pointer" title="View Doc"></i>
                                    </a>`;
                        }
                        else {
                            return null;
                        }
                    }
                        
                  }, "width": "5%"
            },
            {
                "data": "encryptedId", "render": function (data, type, row, meta) {
                    var MailBtn = `<a class="btn btn-sm btn-custom" onclick="PayMailModal('/GetPaymentOwnerEmail?Id=${data}')"><i class="fa-solid fa-envelope" aria-hidden="true" style="cursor:pointer" title="Send Mail"></i> Send Email</a>`
                    if (Role != "Inspector" && Role != "View Only") {
                          if (!url.includes('View')) {
                                if (row.permitStatus == "Inactive") {
                                    return `${MailBtn}`
                                }
                                else {
                                    if (row.status == "Unpaid") {
                                        if (row.amount <= 0) {
                                            return ` <div class="m-75 btn-group" id="docIcons${meta.row}"  role="group">
                                                
                                                <a class="btn btn-sm btn-custom" onclick = OfflinePayment('/GetFeesDetails?id=${data}')>Pay Offline</a>
                                                <a class="btn btn-sm btn-outline-danger" onclick = Cancel('/CancelPayment?id=${data}')>Cancel </a>
                                                ${MailBtn}
                                          </div>`
                                        }
                                        else {
                                            return ` <div class="m-75 btn-group" id="docIcons${meta.row}"  role="group">
                                                <a class="btn btn-sm btn-custom" style="display:none" onclick = JetPayProcess('/PaymentProcess?id=${data}')>JetPay</a>
                                                <a class="btn btn-sm btn-custom" onclick = OfflinePayment('/GetFeesDetails?id=${data}')>Pay Offline</a>
                                                <a class="btn btn-sm btn-outline-danger"  onclick = Cancel('/CancelPayment?id=${data}')>Cancel </a>      
                                                ${MailBtn}
                                          </div>`
                                        }

                                    }
                                    else {
                                        if (row.status == "Paid") {
                                            if (row.feesStat != 11 && row.feesStat != 9) {
                                                //if (row.ispermitFees == false) {

                                                //}
                                                //else {
                                                //    return null;
                                                //}

                                                if (row.isvoidEnabled) {
                                                    return ` <div class="m-75 btn-group" id="docIcons${meta.row}"  role="group">
                                                <a class="btn btn-sm btn-outline-danger"  onclick = Void('/RefundOfflinePayment?id=${data}&pay_action="OVmJOZsWsU"')>Void </a>   
                                                ${MailBtn}
                                          </div>`
                                                }
                                                else {
                                                    return ` <div class="m-75 btn-group" id="docIcons${meta.row}"  role="group">                                                
                                                <a class="btn btn-sm btn-outline-primary"  onclick = Refund('/RefundOfflinePayment?id=${data}&pay_action="vUZbE7i6A2"')>Refund </a>
                                                ${MailBtn}
                                          </div>`
                                                }

                                            }
                                            else {
                                                return `${MailBtn}`
                                                //return null;
                                            }
                                        }
                                        else {
                                            return `${MailBtn}`
                                            //return null;
                                        }
                                    }
                                }
                                
                          }
                          else {
                                return null;
                          }
                    }

                    else {
                          //return null;
                          return `${MailBtn}`
                    }
                },
                "width": "10%"
            }
            /*{ "data": "message", "width": "20%" },*/
        ],
        "columnDefs": [
            {
                targets: [4],
                render: function (data, type, row) {
                    if (row.status == "Paid" || row.status == "Cancelled" || row.status == "Voided" || row.status == "Refunded") {
                        return "--"
                    }
                    return data + " Days";
                }
            },

        ],
        "language": {
            "emptyTable": "No records found"
        },
          "width": "100%",
          "createdRow": function (row, data, dataIndex) {
                if ($(data)[0].status == "Cancelled") {
                      $(row).css('color', 'red');
                }
                if ($(data)[0].status == "Voided") {
                      $(row).css('color', 'red');
                }
                if ($(data)[0].status == "Paid") {
                      $(row).css('color', 'green');
                }
                if ($(data)[0].status == "Refunded") {
                    $(row).css('color', 'green');
                }
          }
    });
}


function PayMailModal(url) {
    $('#paySendMail').trigger("reset");
    //$('#IID').val(InspectionId);
    $.ajax({
        type: "GET",
        url: url,
        //data: InspectionId,
        success: function (data) {
            console.log(data);
            if (data.success) {
                $('#payMailId').val(data.paymentId);
                $('#paymentMailStatus').val(data.paymentStatus);
                $('#PayToMail').val(data.ownerEmail);

            }
        },
        complete: function () {
            $('#paymailSend').modal('show');
        }

    })
}


function payMailModalert() {
    var flg = 0;
    //console.log($('#purpose').val());
    if ($('#PayToMail').val() != "" || $('#PayCCmail').val() != "" || $('#Paysub').val() != "" || $('#Paybdy').val() != "") {
        flg = 1;
    }
    if (flg == 1) {
        if (confirm("You have some unsaved Changes. On closing, your progress will be lost")) {
            $('#paymailSend').modal('hide');
        }
    }
    else {
        $('#paymailSend').modal('hide');
    }
}


function SendPayMail() {
    let regex = new RegExp("([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\"\(\[\]!#-[^-~ \t]|(\\[\t -~]))+\")@([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\[[\t -Z^-~]*])");
    var flg = 0;
    if ($('#PayToMail').val() == null || $('#PayToMail').val() == "") {
        $('#paytoEmailError').text("\u24d8 This Field is Required");
        setTimeout(() => {
            $('#paytoEmailError').html("");
        }, 5000)
        flg = 1;
    }
    else {
        var str = $('#PayToMail').val();
        var mailids = str.split(',');
        for (let i = 0; i < mailids.length; i++) {
            if (regex.test(mailids[i]) == false) {
                $('#paytoEmailError').text("\u24d8 One or more Email Id is invalid");
                setTimeout(() => {
                    $('#paytoEmailError').html("");
                }, 5000)
                flg = 1;
            }
        }
    }
    if ($('#Paysub').val() == null || $('#Paysub').val() == "") {
        $('#paysubError').text("\u24d8 This Field is Required");
        setTimeout(() => {
            $('#paysubError').html("");
        }, 5000)
        flg = 1;
    }
    if (flg == 1) {
        return false;
    }
    else {
        $('#sendPayMailBtn').prop('disabled', true)
        $.ajax({
            type:"POST",
            url: "/SendPayDetailsViaEmail",
            data: $('#paySendMail').serialize(),
            beforeSend: function () {
                $('div#loading-wrapper').show();
                $('#spin').show();
                $('#saveicon').hide();
                //$('#sendBtn').prop('disabled', true)
                $('#paymailSend').trigger('reset');
            },
            success: function (data) {
                if (data.success) {
                    $('audio#success_sound')[0].play();
                    setTimeout(() => {
                        //toastr.info(data.msg);
                        toastr.info(data.msg);
                    }, 500)
                }
                else {
                    $('#mailSend').modal('hide');
                    $('audio#errorsound')[0].play();
                    setTimeout(() => {
                        toastr.error(data.msg);
                    }, 775)
                }
            },
            complete: function () {
                $('div#loading-wrapper').hide();
                $('#spin').hide();
                $('#saveicon').show();
                $('#paymailSend').modal('hide');
                $('#sendPayMailBtn').prop('disabled', false)
            }
        })
    }
}

function Cancel(url) {    
    
    var title = "";
    var text = "";
    var confirmButtonText = "";
    var cancelButtonText = "";
    var successmsg = "";
    var errormsg = "";

    title = "Are you sure?";
    text = "You won't be able to revert this!";
    confirmButtonText = "Yes Proceed!";
    cancelButtonText = "Cancel";
    successmsg = "Cancelled Successfully";
    errormsg = "Unexpected Error Occurred";

    $('audio#warning')[0].play();
    setTimeout(() => {
        Swal.fire({
            title: title,
            text: text,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#7aa1db',
            cancelButtonColor: '#d33',
            cancelButtonText: cancelButtonText,
            confirmButtonText: confirmButtonText
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "POST",
                    url: url,
                    beforeSend: function () {
                        $('div#loading-wrapper').show();
                    },
                    success: function (data) {
                        if (data.success) {
                            //paymentDataTable.ajax.reload();

                            //if ($('#permitStatusId').val() == 7) {
                            //    StatusChange();
                            //}

                            $('audio#success_sound')[0].play();
                            setTimeout(() => {
                                toastr.success(successmsg);
                            }, 500)
                            $('#paymentAddBtn').prop('disabled', false);
                            paymentDataTable.ajax.reload();

                        }
                        else {
                            $('audio#errorsound')[0].play();
                            setTimeout(() => {
                                toastr.error(errormsg);
                            }, 775)
                        }
                    },
                    error: function (data) {
                        console.log(data);
                    },
                    complete: function () {
                        $('div#loading-wrapper').hide();
                    }
                });
            }
        })
    }, 100)
}


function Refund(url) {

    var title = "";
    var text = "";
    var confirmButtonText = "";
    var cancelButtonText = "";
    var successmsg = "";
    var errormsg = "";

    title = "Are you sure you want to Refund this invoice?";
    text = "This action is permanent, and cannot be undone!!";
    confirmButtonText = "Refund Invoice";
    cancelButtonText = "Cancel";
    successmsg = "Refunded Successfully";
    errormsg = "Unexpected Error Occurred";

    $('audio#warning')[0].play();
    setTimeout(() => {
        Swal.fire({
            title: title,
            text: text,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#7aa1db',
            cancelButtonColor: '#d33',
            cancelButtonText: cancelButtonText,
            confirmButtonText: confirmButtonText
        }).then((result) => {
            if (result.isConfirmed) {
                const inputValue = "";
                Swal.fire({
                    title: "Specify the reason behind refunding this transaction",
                    input: "text",
                    //inputLabel: "Your Response",
                    inputValue,
                    showCancelButton: true,
                    inputValidator: (value) => {
                        if (!value) {
                              return "Please Specify the reason behind refunding this transaction!";
                        }
                        else {
                            console.log(value);
                            var formData = new FormData();
                            formData.append('value', value)
                            $.ajax({
                                type: "POST",
                                url: url,
                                data: formData,
                                contentType: false,
                                processData: false,
                                beforeSend: function () {
                                    $('div#loading-wrapper').show();
                                },
                                success: function (data) {
                                    if (data.success) {
                                        $('audio#success_sound')[0].play();
                                        setTimeout(() => {
                                            toastr.success(successmsg);
                                        }, 500)
                                        $('#paymentAddBtn').prop('disabled', false);
                                        paymentDataTable.ajax.reload();

                                    }
                                    else {
                                        $('audio#errorsound')[0].play();
                                        setTimeout(() => {
                                            toastr.error(errormsg);
                                        }, 775)
                                    }
                                },
                                error: function (data) {
                                    console.log(data);
                                },
                                complete: function () {
                                    $('div#loading-wrapper').hide();
                                }
                            });
                        }
                    }
                });
            }
        })
    }, 100)
}


function Void(url) {

    var title = "";
    var text = "";
    var confirmButtonText = "";
    var cancelButtonText = "";
    var successmsg = "";
    var errormsg = "";

    title = "Are you sure you want to VOID this invoice?";
    text = "This action is permanent, and cannot be undone!!";
    confirmButtonText = "VOID Transaction";
    cancelButtonText = "Cancel";
    successmsg = "Voided Successfully";
    errormsg = "Unexpected Error Occurred";

    $('audio#warning')[0].play();
    setTimeout(() => {
        Swal.fire({
            title: title,
            text: text,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#7aa1db',
            cancelButtonColor: '#d33',
            cancelButtonText: cancelButtonText,
            confirmButtonText: confirmButtonText
        }).then((result) => {
            if (result.isConfirmed) {
                const inputValue = "";
                Swal.fire({
                    title: "Specify the reason behind voiding this transaction",
                    input: "text",
                    //inputLabel: "Your Response",
                    inputValue,
                    showCancelButton: true,
                    inputValidator: (value) => {
                        if (!value) {
                            return "Please Specify the reason behind voiding this transaction!";
                        }
                        else {
                            console.log(value);
                            var formData = new FormData();
                            formData.append('value', value)
                            $.ajax({
                                type: "POST",
                                url: url,
                                data: formData,
                                contentType: false,
                                processData: false,
                                beforeSend: function () {
                                    $('div#loading-wrapper').show();
                                },
                                success: function (data) {
                                    if (data.success) {
                                        //paymentDataTable.ajax.reload();

                                        //if ($('#permitStatusId').val() == 7) {
                                        //    StatusChange();
                                        //}

                                        $('audio#success_sound')[0].play();
                                        setTimeout(() => {
                                            toastr.success(successmsg);
                                        }, 500)
                                        $('#paymentAddBtn').prop('disabled', false);
                                        paymentDataTable.ajax.reload();

                                    }
                                    else {
                                        $('audio#errorsound')[0].play();
                                        setTimeout(() => {
                                            toastr.error(errormsg);
                                        }, 775)
                                    }
                                },
                                error: function (data) {
                                    console.log(data);
                                },
                                complete: function () {
                                    $('div#loading-wrapper').hide();
                                }
                            });
                        }
                    }
                });
                //const ipAPI = "//api.ipify.org?format=json";
                //const response = await fetch(ipAPI);
                //const data = await response.json();
                //const inputValue = data.ip;
                //const { value: ipAddress } = await Swal.fire({
                //    title: "Enter your IP address",
                //    input: "text",
                //    inputLabel: "Your IP address",
                //    inputValue,
                //    showCancelButton: true,
                //    inputValidator: (value) => {
                //        if (!value) {
                //            return "You need to write something!";
                //        }
                //    }
                //});
                //if (ipAddress) {
                //    Swal.fire(`Your IP address is ${ipAddress}`);
                //}
                //$.ajax({
                //    type: "POST",
                //    url: url,
                //    beforeSend: function () {
                //        $('div#loading-wrapper').show();
                //    },
                //    success: function (data) {
                //        if (data.success) {
                //            //paymentDataTable.ajax.reload();

                //            //if ($('#permitStatusId').val() == 7) {
                //            //    StatusChange();
                //            //}

                //            $('audio#success_sound')[0].play();
                //            setTimeout(() => {
                //                toastr.success(successmsg);
                //            }, 500)
                //            $('#paymentAddBtn').prop('disabled', false);
                //            paymentDataTable.ajax.reload();

                //        }
                //        else {
                //            $('audio#errorsound')[0].play();
                //            setTimeout(() => {
                //                toastr.error(errormsg);
                //            }, 775)
                //        }
                //    },
                //    error: function (data) {
                //        console.log(data);
                //    },
                //    complete: function () {
                //        $('div#loading-wrapper').hide();
                //    }
                //});
            }
        })
    }, 100)
}

function JetPayProcess(url) {

    var title = "";
    var text = "";
    var confirmButtonText = "";
    var cancelButtonText = "";
    var successmsg = "";
    var errormsg = "";

    title = "Are you sure you want to pay online?";
    text = "You will be paying through JetPay";
    confirmButtonText = "Yes Proceed!";
    cancelButtonText = "Cancel";
    successmsg = "Payment Link is sent to your registered EmailId. Please check your email to proceed with payment";
    errormsg = "Payment Link already Sent to your Email..Cannot Resend Link";

    $('audio#warning')[0].play();
    setTimeout(() => {
        Swal.fire({
            title: title,
            text: text,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#7aa1db',
            cancelButtonColor: '#d33',
            cancelButtonText: cancelButtonText,
            confirmButtonText: confirmButtonText
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "POST",
                    url: url,
                    beforeSend: function () {
                        $('div#loading-wrapper').show();
                    },
                    success: function (data) {
                        if (data.success) {
                            //paymentDataTable.ajax.reload();

                            //if ($('#permitStatusId').val() == 7) {
                            //    StatusChange();
                            //}

                            $('audio#success_sound')[0].play();
                            setTimeout(() => {
                                toastr.info(successmsg);
                            }, 500)
                            //$('#Cnt').val(parseInt($('#Cnt').val()) - 1);
                        }
                        else {
                            $('audio#errorsound')[0].play();
                            setTimeout(() => {
                                toastr.error(errormsg);
                            }, 775)
                        }
                    },
                    error: function (data) {
                        console.log(data);
                    },
                    complete: function () {
                        $('div#loading-wrapper').hide();
                    }
                });
            }
        })
    }, 100)
}

//function funPayment(url) {

//    var title = "";
//    var text = "";
//    var confirmButtonText = "";
//    var cancelButtonText = "";
//    var successmsg = "";
//    var errormsg = "";

//    title = "Are you sure?";
//    text = "You won't be able to revert this!";
//    confirmButtonText = "Yes Proceed!";
//    cancelButtonText = "Cancel";
//    successmsg = "Payment Successfully";
//    errormsg = "Unexpected Error Occurred";

//    $('audio#warning')[0].play();
//    setTimeout(() => {
//        Swal.fire({
//            title: title,
//            text: text,
//            icon: 'warning',
//            showCancelButton: true,
//            confirmButtonColor: '#7aa1db',
//            cancelButtonColor: '#d33',
//            cancelButtonText: cancelButtonText,
//            confirmButtonText: confirmButtonText
//        }).then((result) => {
//            if (result.isConfirmed) {
//                $.ajax({
//                    type: "POST",
//                    url: url,
//                    beforeSend: function () {
//                        $('div#loading-wrapper').show();
//                    },
//                    success: function (data) {
//                        if (data.success) {
//                            //paymentDataTable.ajax.reload();

//                            //if ($('#permitStatusId').val() == 7) {
//                            //    StatusChange();
//                            //}

//                            $('audio#success_sound')[0].play();
//                            setTimeout(() => {
//                                toastr.success(successmsg);
//                            }, 500)
//                            $('#Cnt').val(parseInt($('#Cnt').val()) - 1);
//                        }
//                        else {
//                            $('audio#errorsound')[0].play();
//                            setTimeout(() => {
//                                toastr.error(errormsg);
//                            }, 775)
//                        }
//                    },
//                    error: function (data) {
//                        console.log(data);
//                    },
//                    complete: function () {
//                        $('div#loading-wrapper').hide();
//                    }
//                });
//            }
//        })
//    }, 100)
//}


function OfflinePayment(url)
{
    $('#OfflinePaymentForm').trigger('reset');
    //$('#ChequeDetailsSection').hide();
    //$('#CardDetailsSection').hide();
    
    $('#ReferenceDetailsSection').hide();
    $('#offlinePayModal').modal('show');
    $.ajax({
        type: "GET",
        url: url,
        success: function (data) {
            console.log(data)
            $('#invoiceOffLineNo').val(data.payment.invoiceNo);
            $('#amount').val(data.payment.amount);
            $('#offlinepayamountDue').val(data.payment.amount);
            //$('#totalAmt').val(data.payment.amount);
            $('#payfeesID').val(data.payment.id);
            $('#payEstID').val(data.payment.establishmentId);
            
        },
        complete: function () {
              //if (parseFloat($("#itemAmt").val()) < parseFloat($('#amount').val())) {
              //      $('#OfflinePaySubmitBtn').prop('disabled', true)
              //}
              validateAmount()
        }
    })
}

//function validateAmount() {
//      let totalAmount = parseFloat($('#amount').val()) || 0;
//      let enteredAmount = parseFloat($('#itemAmt').val()) || 0;

//      if (enteredAmount < totalAmount) {
//            $('#OfflinePaySubmitBtn').prop('disabled', true);
//      }
//      else if (enteredAmount == totalAmount) {
//            $('#OfflinePaySubmitBtn').prop('disabled', false);
//      }
//      else
//      {
//            $('#OfflinePaySubmitBtn').prop('disabled', false);
//      }
//}

function validateAmount() {
      let totalAmount = parseFloat($('#amount').val()) || 0;
      let totalEnteredAmount = parseFloat($('#totalAmt').val()) || 0;

      if (totalEnteredAmount < totalAmount) {
            $('#OfflinePaySubmitBtn').prop('disabled', true);  
      } else if (totalEnteredAmount === totalAmount) {
            $('#OfflinePaySubmitBtn').prop('disabled', false);  
      } else {
            $('#OfflinePaySubmitBtn').prop('disabled', true);  
      }
}

function SelectedFunc(element, count, code)
{
    var latefine = 0;
    if (code == "TF") {
        var totalcnt = $('#totalcnt').val();
        //var totalcnt = $('#subTotal').val();
        for (var i = 0; i < totalcnt; i++)
        {
            $('input[type="hidden"][name="FeesList[' + i + '].IsSelected"]').prop("checked", false);
            $('input[type="hidden"][name="FeesList[' + i + '].IsSelected"]').prop("value", "false");
            /*$('#chk[1]').prop('checked', false);*/
            //$('#tstchk').prop("checked", false);
            //$('#chk[' + i + ']').prop("checked", false);
        }

        //$('input:checkbox[name=chkTempPrice]').attr('checked', false);
        //$('input:checkbox[name=chkTempPrice]').removeAttr('checked');  
        $('input:checkbox[name=chkTempPrice]').prop("checked", false);
        //$('#totalamt').val(0)
        $('#subTotal').val(0)
        //if ($('#latefine').val() != '')
        //{
        //    latefine = parseFloat($('#latefine').val());
        //    //$('#totalamt').val(latefine);
        //    $('#subTotal').val(latefine);
        //}
        
    }

    
    //var totalamt = parseFloat($('#totalamt').val());
    var totalamt = parseFloat($('#subTotal').val());
    var amt = parseFloat($('input[type="hidden"][name="FeesList[' + count + '].Amount"]').val());
    if (element == true) {
        $('input:checkbox[id=chk_' + count + ']').prop('checked', true);
        $('input[type="hidden"][name="FeesList[' + count + '].IsSelected"]').prop("checked", true);
        $('input[type="hidden"][name="FeesList[' + count + '].IsSelected"]').prop("value", "true");

        
        totalamt += amt;
        //$('#totalamt').val(totalamt)
        $('#subTotal').val(totalamt)
        //console.log($('input[type="hidden"][name="FeesList[' + count + '].EstablishmentTypeId"]').val());
    }
    else {
        $('input:checkbox[id=chk_' + count + ']').prop('checked', false);
        $('input[type="hidden"][name="FeesList[' + count + '].IsSelected"]').prop("checked", false);
        $('input[type="hidden"][name="FeesList[' + count + '].IsSelected"]').prop("value", "false");
        totalamt -= amt;
        if (code == 'TF') {
              //$('#totalamt').val(0)
              $('#subTotal').val(0)
              //if ($('#latefine').val() != '') {
              //   var latefine = parseFloat($('#latefine').val());
              //   //$('#totalamt').val(latefine);
              //    $('#subTotal').val(latefine);
              //}
            //var latefine = parseInt($('#latefine').val());
            //$('#totalamt').val(latefine);
        }
        else {
              //$('#totalamt').val(totalamt)
            $('#subTotal').val(totalamt)
        }
    }
    $('#totalamt').val(parseFloat($('#subTotal').val()) + parseFloat($('#hdnMiscelliniusFees').val()) + parseFloat($('#hdnlatefine').val()))
    if ($('#subTotal').val() == 0) {
        $(".show-radio").prop("disabled", true);
        $('#miscelliniusfees').prop('readonly', true)
        $('#miscelliniusfeesTitle').prop('readonly', true)
        $('#miscelliniusfees').val(0)
        $('#hdnMiscelliniusFees').val(0)
        $('#miscelliniusfeesTitle').val('')
        //$('.show-radio').prop('disabled', true)
    }
    else {
        $(".show-radio").prop("disabled", false);
        //$('#miscelliniusfees').prop('readonly', false)
        //$('#miscelliniusfeesTitle').prop('readonly', false)
    }
}

function Misc(val) {
    
    if (val == '') {
        alert(1);
        $('#miscelliniusfees').val(0);
        $('#hdnMiscelliniusFees').val(0);
    }
    $('#totalamt').val(parseFloat($('#subTotal').val()) + parseFloat($('#hdnlatefine').val()))
    var type = $('#adjFeesType').val();
    if (type == "Discount") {
        //console.log(type);
        $('#hdnMiscelliniusFees').val(-val);
        $('#miscelliniusfees').val(-val);
    }
    else if (type == "Additional") {
        $('#hdnMiscelliniusFees').val(val);
        $('#miscelliniusfees').val(val);
        //$('#totalamt').val(parseFloat($('#totalamt').val()) + val)
    }
    //$('#totalamt').val(parseFloat($('#totalamt').val()) + parseFloat($('#hdnMiscelliniusFees').val()))
    if ($('#miscelliniusfees').val() != '' || $('#miscelliniusfees').val() != 0) {
        $('#totalamt').val(parseFloat($('#subTotal').val()) + parseFloat($('#hdnMiscelliniusFees').val()) + parseFloat($('#hdnlatefine').val()))
    }
    
}

function radiodis() {
    if ($('#miscelliniusfees').val() != 0) {
        $('#miscelliniusfees').val(-Math.abs($('#miscelliniusfees').val()))
        $('#hdnMiscelliniusFees').val(-Math.abs($('#hdnMiscelliniusFees').val()))
    }
    else {
        $('#miscelliniusfees').val(0)
        $('#hdnMiscelliniusFees').val(0)
    }
    $('#totalamt').val(parseFloat($('#subTotal').val()) + parseFloat($('#hdnMiscelliniusFees').val()) + parseFloat($('#hdnlatefine').val()))
    //$('#miscelliniusfeesTitle').val('')
    $('#addtn').prop('checked', false);
    $('#miscelliniusfees').prop('readonly', false)
    $('#miscelliniusfeesTitle').prop('readonly', false)
    $('#adjFeesType').val("Discount");
}
function radioadd() {
    if ($('#miscelliniusfees').val() != 0) {
        $('#miscelliniusfees').val(Math.abs($('#miscelliniusfees').val()))
        $('#hdnMiscelliniusFees').val(Math.abs($('#hdnMiscelliniusFees').val()))
    }
    else {
        $('#miscelliniusfees').val(0)
        $('#hdnMiscelliniusFees').val(0)
    }
    $('#totalamt').val(parseFloat($('#subTotal').val()) + parseFloat($('#hdnMiscelliniusFees').val()) + parseFloat($('#hdnlatefine').val()))
    //$('#miscelliniusfeesTitle').val('')
    $('#discnt').prop('checked', false);
    $('#miscelliniusfees').prop('readonly', false)
    $('#miscelliniusfeesTitle').prop('readonly', false)
    $('#adjFeesType').val("Additional");
}
function radioclr() {
    $('#clr').prop('checked', false);
    $('#discnt').prop('checked', false);
    $('#addtn').prop('checked', false);
    $('#miscelliniusfees').prop('readonly', true)
    $('#miscelliniusfees').val(0)
    $('#hdnMiscelliniusFees').val(0)
    $('#miscelliniusfeesTitle').prop('readonly', true)
    $('#miscelliniusfeesTitle').val('')
    $('#adjFeesType').val("");
    $('#totalamt').val(parseFloat($('#subTotal').val()) + parseFloat($('#hdnlatefine').val()))
}


function ShowHideAdjuncData(val) {
    if (val == 3 || val == 4 || val == 5) {
        $('#ReferenceDetailsSection').show();
        //$('#CardDetailsSection').hide();
        //MAX LIMIT SET
        //$('#referenceNumber').prop('maxlength', 12)
    }
    //else if (val == 4) {
    //    $('#ReferenceDetailsSection').show();
    //    $('#referenceNumber').prop('maxlength', 10)
    //    //$('#CardDetailsSection').show();
    //    //$('#ChequeDetailsSection').hide();
    //    //$('#cardNumber').prop('maxlength', 10)
    //}
    else {
        //$('#ChequeDetailsSection').hide();
        //$('#CardDetailsSection').hide();
        $('#ReferenceDetailsSection').hide();
    }
}

function GetInvoice() {
    $.ajax({
        type: "GET",
        url: '/GetInvoiceNumber',
        success: function (data) {
            //console.log(data)
            if (data.success) {
                $('#invoiceNo').val(data.invoice);
            }
            
        },
        error: function (data) {
            console.log(data);
        },
        complete: function () {

        }
    })
}

/// Old JS code Start

//function PaymentSubmit()
//{
//    $('#PaysaveBtn').prop('disabled', true);
//    var flg = 0;
//    var totalcnt = $('#totalcnt').val();
//    for (let i = 0; i < totalcnt; i++) {

//        if ($('input:checkbox[id=chk_'+ i +']').is(":checked")) {
//                flg = 1;
//                break;
//        }
//    }
//    if (flg == 0) {

//        $('audio#errorsound')[0].play();
//        setTimeout(() => {
//            toastr.error("Please Select a Fee To Proceed");
//        }, 775)
//        $('#PaysaveBtn').prop('disabled', false);
//        return false;
//    }
//    else {
//          flg = 0
//          if ($('#adjFeesType').val() != "") {
//                if ($('#miscelliniusfeesTitle').val() == "") {

//                }
//                if ($('#miscelliniusfeesTitle').val() == "") {
//                      $('#miscelliniusfeesTitle').css('border-color', 'red')
//                      //$('#miscelliniusfeesTitleErr').text("\u24d8 Required");
//                      setTimeout(() => {
//                            $('#miscelliniusfeesTitle').css('border-color', '');
//                      }, 4000)
//                      flg = 1;
//                }
//                if ($('#miscelliniusfees').val() == 0) {
//                      $('#miscelliniusfees').css('border-color', 'red')
//                      //$('#miscelliniusfeesErr').text("\u24d8 Required");
//                      setTimeout(() => {
//                            //$('#miscelliniusfeesErr').html("");
//                            $('#miscelliniusfees').css('border-color', '')
//                      }, 4000)
//                      flg = 1;
//                }
//          }

//          if (flg == 1) {
//                $('audio#errorsound')[0].play();
//                setTimeout(() => {
//                      toastr.error("Please Fill out required Fields");
//                }, 775)
//                $('#PaysaveBtn').prop('disabled', false);
//                return false;
//          }
//          else {
//                $.ajax({
//                      type: "POST",
//                      url: '/SaveFees',
//                      data: $('#PaymentForm').serialize(),
//                      beforeSend: function () {
//                            $('div#loading-wrapper').show();
//                            $('#PaysaveBtn').prop('disabled', true);
//                      },
//                      success: function (data) {
//                            if (data.success) {
//                                  $('audio#success_sound')[0].play();
//                                  setTimeout(() => {
//                                        toastr.success("Successfully Saved");
//                                  }, 500)
//                                  if (data.msg == "Single") {
//                                      $('#paymentAddBtn').prop('disabled', true);
//                                  }
//                                  else {
//                                      $('#paymentAddBtn').prop('disabled', false);
//                                  }
//                                  if (data.permitstatusid == 7 && data.btn!='')
//                                  {
//                                      $(`<i class="fa fa-check-circle"></i>`).insertAfter('#sb' + data.permitstatusid)
//                                      $('#s' + data.permitStatusId).addClass("completed");

//                                      $('#permitStatusId').val(data.permitstatusid);
//                                      $('#permitStatusflag').val(data.permitStatusId)
//                                      $('#oldPermitStatusId').val(data.oldpermitstatus);
//                                      $('#statusbtn').text("Place in " + data.btn);
//                                  }
//                                location.reload();
//                            }
//                            else {
//                                  $('audio#errorsound')[0].play();
//                                  setTimeout(() => {
//                                        toastr.error("Unexpected Error Occurred");
//                                  }, 775)
//                            }
//                      },
//                      error: function (data) {
//                            console.log(data);
//                      },
//                      complete: function () {
//                            $('div#loading-wrapper').hide();
//                            $('#PaysaveBtn').prop('disabled', false);
//                            $('#paymentAddModal').modal('hide');
//                            paymentDataTable.ajax.reload();
//                      }
//                })
//          }



//    }


//}

/// Old JS code End

///New Code Start
let isSubmitting = false;  // Double submission guard

function PaymentSubmit() {
      // Prevent multiple submissions by checking the flag
      if (isSubmitting) {
            return false;
      }

      $('#PaysaveBtn').prop('disabled', true);

      var flg = 0;
      var totalcnt = $('#totalcnt').val();

      // Check if any checkboxes are selected
      for (let i = 0; i < totalcnt; i++) {
            if ($('input:checkbox[id=chk_' + i + ']').is(":checked")) {
                  flg = 1;
                  break;
            }
      }

      // If no checkboxes are selected
      if (flg == 0) {
            $('audio#errorsound')[0].play();
            setTimeout(() => {
                  toastr.error("Please Select a Fee To Proceed");
            }, 775);
            $('#PaysaveBtn').prop('disabled', false);
            return false;
      }

      // Additional checks for fees
      else {
            flg = 0;
            if ($('#adjFeesType').val() != "") {
                  if ($('#miscelliniusfeesTitle').val() == "") {
                        $('#miscelliniusfeesTitle').css('border-color', 'red');
                        setTimeout(() => {
                              $('#miscelliniusfeesTitle').css('border-color', '');
                        }, 4000);
                        flg = 1;
                  }
                  if ($('#miscelliniusfees').val() == 0) {
                        $('#miscelliniusfees').css('border-color', 'red');
                        setTimeout(() => {
                              $('#miscelliniusfees').css('border-color', '');
                        }, 4000);
                        flg = 1;
                  }
            }

            if (flg == 1) {
                  $('audio#errorsound')[0].play();
                  setTimeout(() => {
                        toastr.error("Please Fill out required Fields");
                  }, 775);
                  $('#PaysaveBtn').prop('disabled', false);
                  return false;
            }

            // Proceed with form submission
            else {
                  isSubmitting = true;  // Set the flag to prevent double submission

                  $.ajax({
                        type: "POST",
                        url: '/SaveFees',
                        data: $('#PaymentForm').serialize(),
                        beforeSend: function () {
                              $('div#loading-wrapper').show();
                              $('#PaysaveBtn').prop('disabled', true);
                        },
                        success: function (data) {
                              if (data.success) {
                                    $('audio#success_sound')[0].play();
                                    setTimeout(() => {
                                          toastr.success("Successfully Saved");
                                    }, 500);

                                    if (data.msg == "Single") {
                                          $('#paymentAddBtn').prop('disabled', true);
                                    } else {
                                          $('#paymentAddBtn').prop('disabled', false);
                                    }

                                    if (data.permitstatusid == 7 && data.btn != '') {
                                          $(`<i class="fa fa-check-circle"></i>`).insertAfter('#sb' + data.permitstatusid);
                                          $('#s' + data.permitStatusId).addClass("completed");

                                          $('#permitStatusId').val(data.permitstatusid);
                                          $('#permitStatusflag').val(data.permitStatusId);
                                          $('#oldPermitStatusId').val(data.oldpermitstatus);
                                          $('#statusbtn').text("Place in " + data.btn);
                                    }

                                    location.reload();
                              } else {
                                    $('audio#errorsound')[0].play();
                                    setTimeout(() => {
                                          toastr.error("Unexpected Error Occurred");
                                    }, 775);
                              }
                        },
                        error: function (data) {
                              console.log(data);
                        },
                        complete: function () {
                              isSubmitting = false;  // Reset the flag after completion
                              $('div#loading-wrapper').hide();
                              $('#PaysaveBtn').prop('disabled', false);
                              $('#paymentAddModal').modal('hide');
                              paymentDataTable.ajax.reload();
                        }
                  });
            }
      }
}

///End Start

function PaySelectModalClose()
{
    $('#paymentAddModal').modal('hide');
}

function PayOfflineModalClose(url) {
    $('#offlinePayModal').trigger('reset')
    $('#tblCustomers > tbody').empty();
    $('#offlineitemCount').val(0);
    $('#totalAmt').val(0);
    $('#payMethodId').empty();
    $('#payMethodId').append('<option disabled selected>--Select--</option>');
    $('#payMethodId').append('<option value="2">Cash</option>');
    $('#payMethodId').append('<option value="3">Check</option>');
    $('#payMethodId').append('<option value="4">Card</option>');
    $('#payMethodId').append('<option value="5">Money Order</option>');
    $("#itemReferenceNumber").prop('disabled', false)    
    $('#offlinePayModal').modal('hide');
}

function PaymentOfflineSubmit()
{
    $('#OfflinePaySubmitBtn').prop('disabled', true)
    var flg = 0;
    //console.log($('#payMethodId').val());
    //if ($('#collectionDt').val() == "")
    //{
    //    $('#collectionDtErr').text("\u24d8 Required Field")
    //    flg = 1;
    //}
    //if ($('#payMethodId').val() == null) {
    //    $('#payMethodIdErr').text("\u24d8 Required Field")
    //    flg = 1;
    //    setTimeout(() => {
    //        $('#payMethodIdErr').text('');
    //    }, 4000)
    //}
    //if ($('#payMethodId').val() == 3 || $('#payMethodId').val() == 4 || $('#payMethodId').val() == 5) {
    //    if ($('#referenceNumber').val() == "") {
    //        $('#referenceNumberErr').text("\u24d8 Required Field")
    //        flg = 1;
    //        setTimeout(() => {
    //            $('#referenceNumberErr').text('');
    //        }, 4000)
    //    }
    //    //else if ($('#referenceNumber').val().length < 8) {
    //    //    $('#referenceNumberErr').text("\u24d8 Invalid")
    //    //    flg = 1;
    //    //    setTimeout(() => {
    //    //        $('#referenceNumberErr').text('');
    //    //    }, 4000)
    //    //}
    //}
    //if ($('#payMethodId').val() == 4) {
    //    if ($('#cardNumber').val() == "") {
    //        $('#cardNumberErr').text("\u24d8 Required Field")
    //        flg = 1;
    //        setTimeout(() => {
    //            $('#cardNumberErr').text('');
    //        }, 4000)
    //    }
    //    else if ($('#cardNumber').val().length < 4) {
    //        $('#cardNumberErr').text("\u24d8 Invalid")
    //        flg = 1;
    //        setTimeout(() => {
    //            $('#cardNumberErr').text('');
    //        }, 4000)
    //    }
    //}
    if (flg == 1) {
        $('#OfflinePaySubmitBtn').prop('disabled', false)
        return false;
    }
    else {
        $.ajax({
            type: "POST",
            url: "/OfflinePayment",
            data: $('#OfflinePaymentForm').serialize(),
            beforeSend: function () {
                $('div#loading-wrapper').show();
                //$('#OfflinePaySubmitBtn').prop('disabled', true)
            },
            success: function (data) {
                if (data.success) {
                    $('audio#success_sound')[0].play();
                    setTimeout(() => {
                        toastr.success("Record Successfully Saved");
                    }, 500)
                    $('#paymentAddBtn').prop('disabled', false);
                    setTimeout(function () {
                        location.reload();
                    }, 1000)

                    if (data.activateflg == "Active") {
                        $(`<i class="fa fa-check-circle"></i>`).insertAfter('#sb' + data.permitStatusId)
                        $('#s' + data.permitStatusId).addClass("completed");

                        $('#permitStatusId').val(data.permitStatusId);
                        $('#permitStatusflag').val(data.permitStatusId)
                        $('#oldPermitStatusId').val(data.oldPermitStatusId);
                    }
                }
                else {
                      $('audio#errorsound')[0].play();
                      if (data.errormsg != null) {
                            setTimeout(() => {
                                  toastr.error(data.errormsg);
                            }, 775)
                      }
                      else {
                            setTimeout(() => {
                                  toastr.error("Unexpected Error Occurred");
                            }, 775)
                      }
                    
                    
                }
            },
            error: function (data) {
                console.log(data);
            },
            complete: function () {
                $('div#loading-wrapper').hide();
                $('#offlinePayModal').modal('hide');
                $('#OfflinePaySubmitBtn').prop('disabled', false)
                paymentDataTable.ajax.reload();
                
            }

        })
    }
}

function CheckPendingPayment()
{
      var encryptedid = $('#encryptedEstId').val();
      $.ajax({
            type: "GET",
            url: '/CheckPendingPayment?id=' + encryptedid,
            success: function (data) {
                  if (data.success)
                  {
                        $('#paymentAddBtn').prop('disabled', true);
                  }
                  
            }
      })
}


$("body").on("click", "#btnAdd", function () {
    var referneceNumber = $("#itemReferenceNumber");
    var Amount = $("#itemAmt");
    var PaymentMode = $("#payMethodId");

    //console.log(PaymentMode.val())
    var validationFlg = 1;

    if (PaymentMode.val() == null) {
        PaymentMode.css('border-color', 'red');
        setTimeout(() => {
            PaymentMode.css('border-color', '');
        }, 4000)
        //$('audio#errorsound')[0].play();
        //setTimeout(() => {
        //    toastr.error("Please Fill Up Location Field");
        //}, 775)
        validationFlg = 0;
    }

    if (Amount.val() == '') {
        Amount.css('border-color', 'red');
        setTimeout(() => {
            Amount.css('border-color', '');
        }, 4000)       
        validationFlg = 0
        //return false;
    }
    if (PaymentMode.val() != null && PaymentMode.val() != 2) {
        if (referneceNumber.val() == '') {
            referneceNumber.css('border-color', 'red');
            setTimeout(() => {
                referneceNumber.css('border-color', '');
            }, 4000)
            validationFlg = 0
        }
    }

    if (parseFloat(Amount.val())>parseFloat($('#amount').val())) {
          Amount.css('border-color', 'red');
          setTimeout(() => {
                Amount.css('border-color', '');
          }, 4000)
          validationFlg = 0
    }

    if ((parseFloat(Amount.val())+parseFloat($('#totalAmt').val()))>parseFloat($('#amount').val())) {
          Amount.css('border-color', 'red');
          setTimeout(() => {
                Amount.css('border-color', '');
          }, 4000)
          validationFlg = 0
    }

    if (parseFloat($('#totalAmt').val())>parseFloat($('#amount').val())) {
          Amount.css('border-color', 'red');
          setTimeout(() => {
                Amount.css('border-color', '');
          }, 4000)
          validationFlg = 0
    }


    if (validationFlg == 0) {
        $('audio#errorsound')[0].play();
        if (parseFloat(Amount.val()) > parseFloat($('#amount').val())) {
            setTimeout(() => {
                    toastr.error("Amount you are trying to insert is greater than the total amount");
            }, 775)
        }
        else if (parseFloat($('#totalAmt').val()) == parseFloat($('#amount').val())) {
            setTimeout(() => {
                    toastr.error("Sum of all amounts entered is already equal to the total amount.. Cannot add more");
            }, 775)
        }
        else if ((parseFloat(Amount.val()) + parseFloat($('#totalAmt').val())) > parseFloat($('#amount').val())) {
              setTimeout(() => {
                    toastr.error("Cannot add amount larger than the required amount");
              }, 775)
        }
        else {
              setTimeout(() => {
                    toastr.error("Please Fill Up Required Fields");
              }, 775)
        }
        return false;
    }



    //var txtCountry = $("#txtCountry");
    var txtCount = $("#offlineitemCount");

    //Get the reference of the Table's TBODY element.
    var tBody = $("#tblCustomers > TBODY")[0];

    //Add Row.
    var row = tBody.insertRow(-1);

    //Add Name cell.
    
    var cell = $(row.insertCell(-1));
    if (PaymentMode.val() == 2) {
        cell.html("Cash");
    }
    else if (PaymentMode.val() == 3) {
        cell.html("Check");
    }
    else if (PaymentMode.val() == 4) {
        cell.html("Card");
    }
    else if (PaymentMode.val() == 5) {
        cell.html("Money Order");
    }
    cell.append("<input type=\"hidden\" id=\"PaymentSplit_PaymentMethod\" name=\"PaymentSplit[" + txtCount.val() + "].PaymentMethod\" value=\"" + PaymentMode.val() + "\">");

    var cell = $(row.insertCell(-1));
    cell.html(referneceNumber.val());
    cell.append("<input type=\"hidden\" id=\"PaymentSplit_ReferenceNumber\" name=\"PaymentSplit[" + txtCount.val() + "].ReferenceNumber\" value=\"" + referneceNumber.val() + "\">");

    var cell = $(row.insertCell(-1));
    cell.html(Amount.val());
    cell.append("<input type=\"hidden\" id=\"PaymentSplit_Amount\" name=\"PaymentSplit[" + txtCount.val() + "].Amount\" value=\"" + Amount.val() + "\">");

   $('#totalAmt').val(parseFloat($('#totalAmt').val()) + parseFloat(Amount.val()))
   $('#offlinepayamountDue').val(parseFloat($('#amount').val())-parseFloat($('#totalAmt').val()))
    //Add Country cell.
    //cell = $(row.insertCell(-1));
    //cell.html(txtCountry.val());
    //cell.append("<input type=\"hidden\" id=\"FinalTemperatureList_Temperature\" name=\"FinalTemperatureList[" + txtCount.val() + "].Temperature\" value=\"" + txtCountry.val() + "\">");

    //document.getElementById("txtCount").val() = txtCount.val() + 1;
    txtCount.val(parseInt(txtCount.val()) + 1);

    //Add Button cell.
    cell = $(row.insertCell(-1));
    var btnRemove = $("<input />");
    btnRemove.attr("type", "button");
    btnRemove.attr("onclick", "Remove(this);");
    btnRemove.addClass('btn btn-sm btn-danger remove valid');

    btnRemove.val("Remove");
    cell.append(btnRemove);

    //Clear the TextBoxes.
    referneceNumber.val("");
    referneceNumber.prop('disabled', false)
    Amount.val("");
    //PaymentMode.find('[value="' + PaymentMode.val() +'"]').remove()
    PaymentMode.val('--Select--');

   validateAmount() 
    
    //txtCountry.val("");

    //alert($('#txtCount').val());
});

//function Remove(button) {
//      //Determine the reference of the Row using the Button.
//      var row = $(button).closest("TR");
//      var name = $("TD", row).eq(0).html();
//      $('audio#warning')[0].play();

//      setTimeout(() => {
//            Swal.fire({
//                  title: 'Are you sure?',
//                  icon: 'warning',
//                  showCancelButton: true,
//                  confirmButtonColor: '#8B9BB2',
//                  cancelButtonColor: '#d33',
//                  confirmButtonText: 'Yes, delete it!'
//            }).then((result) => {
//                  if (result.isConfirmed) {
//                        //Get the reference of the Table.
//                        var table = $("#tblCustomers")[0];

//                        // Retrieve the payment method of the row to be deleted.
//                        var TobeDeletedPaymentMethod = $("input[type='hidden'][name='PaymentSplit[" + (row[0].rowIndex - 1) + "].PaymentMethod']").val();

//                        // Re-add the payment method back to the dropdown after deletion.
//                        if (TobeDeletedPaymentMethod == 2) {
//                              $("#payMethodId").append('<option value="2">Cash</option>');
//                        } else if (TobeDeletedPaymentMethod == 3) {
//                              $("#payMethodId").append('<option value="3">Check</option>');
//                        } else if (TobeDeletedPaymentMethod == 4) {
//                              $("#payMethodId").append('<option value="4">Card</option>');
//                        } else if (TobeDeletedPaymentMethod == 5) {
//                              $("#payMethodId").append('<option value="5">Money Order</option>');
//                        }

//                        // Retrieve the amount of the row to be deleted.
//                        var amountToRemove = parseFloat(row.find("input[name*='.Amount']").val());

//                        // Update the total amount.
//                        $('#totalAmt').val(parseFloat($('#totalAmt').val()) - amountToRemove);

//                        // Delete the row.
//                        table.deleteRow(row[0].rowIndex);

//                        // Decrease the item count.
//                        var cnt = $("#offlineitemCount").val();
//                        $("#offlineitemCount").val(parseInt(cnt) - 1);

//                        // Re-index the remaining rows.
//                        $("#tblCustomers > TBODY > TR").each(function (index) {
//                              var payMethodInput = $(this).find("input[type='hidden'][id='PaymentSplit_PaymentMethod']");
//                              var referenceNumberInput = $(this).find("input[type='hidden'][id='PaymentMethod_ReferenceNumber']");
//                              var amountInput = $(this).find("input[type='hidden'][id='PaymentMethod_Amount']");

//                              payMethodInput.attr("name", "PaymentSplit[" + index + "].PaymentMethod");
//                              referenceNumberInput.attr("name", "PaymentSplit[" + index + "].ReferenceNumber");
//                              amountInput.attr("name", "PaymentSplit[" + index + "].Amount");
//                        });

//                        // Revalidate the total amount after deletion to enable/disable the submit button.
//                        validateAmount();
//                  }
//            });
//      }, 100);
//}

function Remove(button) {
    //Determine the reference of the Row using the Button.
    // alert(button);
    var row = $(button).closest("TR");
    var name = $("TD", row).eq(0).html();
    $('audio#warning')[0].play();
    setTimeout(() => {
        Swal.fire({
            title: 'Are you sure?',

            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#8B9BB2',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                //Get the reference of the Table.
                var table = $("#tblCustomers")[0];
                //console.log(row[0].rowIndex);
                //name =\"PaymentSplit[" + txtCount.val() + "].PaymentMethod\"

                //var TobeDeletedPaymentMethod = $("input[type='hidden'][name='PaymentSplit[" + (row[0].rowIndex - 1) + "].PaymentMethod']").val();

                //console.log(TobeDeletedPaymentMethod);

                //if (TobeDeletedPaymentMethod == 2) {
                //    $("#payMethodId").append('<option value="2">Cash</option>')
                //}
                //else if (TobeDeletedPaymentMethod == 3) {
                //    $("#payMethodId").append('<option value="3">Check</option>')
                //}
                //else if (TobeDeletedPaymentMethod == 4) {
                //    $("#payMethodId").append('<option value="4">Card</option>')
                //}
                //else if (TobeDeletedPaymentMethod == 5) {
                //    $("#payMethodId").append('<option value="5">Money Order</option>')
                //}

                var amountToRemove = parseFloat(row.find("input[name*='.Amount']").val());
                $('#totalAmt').val(parseFloat($('#totalAmt').val()) - amountToRemove);

                $('#offlinepayamountDue').val(parseFloat($('#offlinepayamountDue').val())+amountToRemove)

                //Delete the Table row using it's Index.
                table.deleteRow(row[0].rowIndex);

                var cnt = $("#offlineitemCount").val();
                $("#offlineitemCount").val(parseInt(cnt) - 1);

                $("#tblCustomers > TBODY > TR").each(function (index) {
                    var payMethodinput = $(this).find("input[type='hidden'][id='PaymentSplit_PaymentMethod']");
                    var ReferenceNumberinput = $(this).find("input[type='hidden'][id='PaymentSplit_ReferenceNumber']");
                    var Amountinput = $(this).find("input[type='hidden'][id='PaymentSplit_Amount']");
                    /*console.log(input);*/
                    payMethodinput.attr("name", "PaymentSplit[" + index + "].PaymentMethod");
                    ReferenceNumberinput.attr("name", "PaymentSplit[" + index + "].ReferenceNumber");
                    Amountinput.attr("name", "PaymentSplit[" + index + "].Amount");
                });

                validateAmount();


            }

        })
    }, 100)
    //if (confirm("Do you want to delete: " + name)) {
    //    //Get the reference of the Table.
    //    var table = $("#tblCustomers")[0];

    //    //Delete the Table row using it's Index.
    //    table.deleteRow(row[0].rowIndex);
    //}
};