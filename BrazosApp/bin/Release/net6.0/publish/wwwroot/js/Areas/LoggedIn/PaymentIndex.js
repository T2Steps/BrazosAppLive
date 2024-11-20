var paymentDataTable;

function loadDataTable(Role) {
      var url = window.location.href;
      //console.log(url);
      paymentDataTable = $('#paymentTableIdx').DataTable({
            "responsive": true,
            "lengthChange": true,
            "autoWidth": true,
            "searching": false,
            "deferRender": true,
            "bProcessing": true,
            "pageLength": 50,
            "ajax": {
                  "url": "/Payments/GetAllPayments",
                  "type": "POST",
                  "data": function (d) {
                        var formData = new FormData();
                        formData.append('Name', $('#estName').val());
                        formData.append('Owner', $('#ownName').val());
                        formData.append('Permit', $('#permitNo').val());
                        formData.append('Address', $('#estAddress').val());
                        formData.append('Amount', $('#amountSearch').val());
                        formData.append('InvoiceNo', $('#invNo').val());
                        //formData.append('FromDate', $('#lowerdaternge').val());
                        //formData.append('ToDate', $('#upperdaternge').val());

                        var plainObject = {};
                        formData.forEach(function (value, key) {
                              plainObject[key] = value;
                        });
                        return plainObject;
                  }
            },
            "columns": [
                  {
                        data: 'id',
                        render: function (data, type, row, meta) {
                              //console.log(row);
                              return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                        }, "width": "3%", className: "text-center"
                  },
                  { "data": "permit", "width": "8%", className: "text-left" },
                  { "data": "name", "width": "10%" },
                  { "data": "owner", "width": "10%" },
                  { "data": "address", "width": "10%" },
                  { "data": "invoice_Number", "width": "8%" },
                  { "data": "receipt_Number", "width": "8%" },
                  {
                        "data": "amount", "width": "5%", className: "text-center", 'render': function (data) {
                              //return data.toFixed(2);
                              return parseFloat(data).toLocaleString(undefined, { minimumFractionDigits: 2, maximumFractionDigits: 2 });
                        }
                  },
                  { "data": "status", "width": "5%" },
                  { "data": "paymentOn", "width": "7%" },
                  {
                        "data": "encryptedId", "render": function (data, type, row, meta) {
                              if (!url.includes('View')) {
                                    return `<a id="lnk_${meta.row}" role="button" href="/GetInvoicePdf?id=${data}" target="blank">
                                                <i class="fas fa-file-pdf ml-4" style="color:#022E5F; cursor:pointer" title="View Doc"></i>
                                          </a>`;
                              }
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

                        }, "width": "5%"
                  },
                  {
                        "data": "encryptedId", "render": function (data, type, row, meta) {
                              var MailBtn = `<a class="btn btn-sm btn-custom" onclick="PayMailModal('/GetPaymentOwnerEmail?Id=${data}')"><i class="fa-solid fa-envelope" aria-hidden="true" style="cursor:pointer" title="Send Mail"></i> Send Email</a>`
                              if (Role != "Inspector" && Role != "View Only") {
                                    if (!url.includes('View')) {
                                          
                                          if (row.status == "Unpaid") {
                                                if (row.amount <= 0) {
                                                      return `<div class="m-75 btn-group" id="docIcons${meta.row}"  role="group">                                                
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
                                                      }
                                                }
                                                else {
                                                      return `${MailBtn}`
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
                        "width": "8%"
                  }
            ],
            "columnDefs": [
                  {
                        targets: [6],
                        render: function (data, type, row) {
                              if (data==null) {
                                    return "--"
                              }
                              return data;
                        }
                  },
                  {
                        targets: [9],
                        "type": "date",
                        render: function (data, type, row) {
                              if (data != null) {
                                    var payDate = data.split("T");
                                    payDate = moment(payDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                                    return payDate;
                              }
                              else {
                                    return null;
                              }
                        }
                  },
            ],
            "language": {
                  "emptyTable": "No Records found"
            },
            "width": "100%",
      });
}






