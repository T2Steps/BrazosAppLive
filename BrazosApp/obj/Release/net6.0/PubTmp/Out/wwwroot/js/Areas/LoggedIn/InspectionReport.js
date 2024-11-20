var dataTable;

function loadDataTable(code, Role) {
      var columns = [];
      var columnDefs = [];
      if (Role == "Inspector") {
            columns = [
                  {
                        data: 'id',
                        render: function (data, type, row, meta) {
                              //console.log(row);
                              return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                        }, "width": "5%", className: "text-center"
                  },
                  { "data": "permit", "width": "10%", className: "text-left" },
                  { "data": "name", "width": "30%" },
                  { "data": "purpose", "width": "10%" },
                  { "data": "inspectionDate", "width": "10%" },
                  { "data": "score", "width": "5%", className: "text-center" },
                  { "data": "followUp", "width": "10%", className: "text-center" },
                  { "data": "followUpDate", "width": "10%" },

                  //{
                  //      "data": "encryptedId", "render": function (data, type, row, meta) {
                  //            var Pdfurl = "";
                  //            var editBtn = '';
                  //            var downLoadBtn = '';
                  //            if (row.purpose == "Opening Inspection") {
                  //                  Pdfurl = "/GetOpeningInspectionPdf"
                  //                  editBtn = '<a class="btn btn-sm btn-custom"  href="/EditOpeningInspection?id=' + data + '" target="blank"><i class="fas fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit Inspection"></i></a>';
                  //            }
                  //            else if (row.purpose == "Walk Through") {
                  //                  Pdfurl = "/GetWalkThroughInspectionPdf"
                  //                  editBtn = '';
                  //            }
                  //            else {
                  //                  Pdfurl = "/GetInspectionPdf"
                  //                  editBtn = '<a class="btn btn-sm btn-custom"  href="/EditInspection?id=' + data + '" target="blank"><i class="fas fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit Inspection"></i></a>'
                  //                  if (row.purpose != "Routine") {
                  //                        downLoadBtn = `<a class="btn btn-sm btn-custom"  href="/DownloadInspectionCommentCertificatePdf?id=${data}" target="blank"><i class="fa-solid fa-download" aria-hidden="true" style="cursor:pointer" title="Download Comment Section"></i></a>`
                  //                  }
                  //            }
                  //            return `<div class="m-75 btn-group"  role="group">
                  //                    ${editBtn}
                  //                    ${downLoadBtn}
                  //                    <a class="btn btn-sm btn-custom"  href="${Pdfurl}?id=${data}" target="blank"><i class="fas fa-file-pdf" aria-hidden="true" style="cursor:pointer" title="View Report"></i></a>
                  //                    <a class="btn btn-sm btn-custom" onclick="MailModal('/GetEstOwnerEmail?InspectionId=${data}')"><i class="fa-solid fa-envelope" aria-hidden="true" style="cursor:pointer" title="Send Mail"></i></a>
                                      
                  //              </div>`
                  //            //return null
                  //      }, "width": "5%"

                  //}
            ]

            columnDefs = [
                  //{
                  //      targets: [4],
                  //      "type": "date",
                  //      render: function (data, type, row) {
                  //            var insDate = data.split("T");
                  //            insDate = moment(insDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                  //            return insDate;
                  //      }
                  //},
                  {
                        targets: [5],
                        "type": "num",
                        render: function (data, type, row) {
                              if (data == null) {
                                    return '--'
                              }
                              return data
                        }
                  },
                  {
                        targets: [6],
                        render: function (data, type, row) {
                              if (data == true) {
                                    return `<p class="badge bg-danger">YES</p>`
                              }
                              return `<p class="badge bg-success">No</p>`
                        }
                  },
                  //{
                  //      targets: [7],
                  //      "type": "date",
                  //      render: function (data, type, row) {
                  //            if (data != null) {
                  //                  var insDate = data.split("T");
                  //                  insDate = moment(insDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                  //                  return insDate;
                  //            }
                  //            return "---"
                  //      }
                  //},
            ]
      }
      else {
            columns = [
                  {
                        data: 'id',
                        render: function (data, type, row, meta) {
                              //console.log(row);
                              return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                        }, "width": "5%", className: "text-center"
                  },
                  { "data": "permit", "width": "10%", className: "text-left" },
                  { "data": "name", "width": "20%" },
                  { "data": "purpose", "width": "10%" },
                  { "data": "inspectionDate", "width": "10%" },
                  { "data": "inspectedBy", "width": "10%" },
                  { "data": "score", "width": "5%", className: "text-center" },
                  { "data": "followUp", "width": "10%", className: "text-center" },
                  { "data": "followUpDate", "width": "10%" },

                  //{
                  //      "data": "encryptedId", "render": function (data, type, row, meta) {
                  //            var Pdfurl = "";
                  //            var editBtn = '';
                  //            var downLoadBtn = '';
                  //            if (row.purpose == "Opening Inspection") {
                  //                  Pdfurl = "/GetOpeningInspectionPdf"
                  //                  editBtn = '<a class="btn btn-sm btn-custom"  href="/EditOpeningInspection?id=' + data + '" target="blank"><i class="fas fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit Inspection"></i></a>';
                  //            }
                  //            else if (row.purpose == "Walk Through") {
                  //                  Pdfurl = "/GetWalkThroughInspectionPdf"
                  //                  editBtn = '';
                  //            }
                  //            else {
                  //                  Pdfurl = "/GetInspectionPdf"
                  //                  editBtn = '<a class="btn btn-sm btn-custom"  href="/EditInspection?id=' + data + '" target="blank"><i class="fas fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit Inspection"></i></a>'
                  //                  if (row.purpose != "Routine") {
                  //                        downLoadBtn = `<a class="btn btn-sm btn-custom"  href="/DownloadInspectionCommentCertificatePdf?id=${data}" target="blank"><i class="fa-solid fa-download" aria-hidden="true" style="cursor:pointer" title="Download Comment Section"></i></a>`
                  //                  }
                  //            }
                  //            return `<div class="m-75 btn-group"  role="group">
                  //                    ${editBtn}
                  //                    ${downLoadBtn}
                  //                    <a class="btn btn-sm btn-custom"  href="${Pdfurl}?id=${data}" target="blank"><i class="fas fa-file-pdf" aria-hidden="true" style="cursor:pointer" title="View Report"></i></a>
                  //                    <a class="btn btn-sm btn-custom"  onclick="MailModal('/GetEstOwnerEmail?InspectionId=${data}')"><i class="fa-solid fa-envelope" aria-hidden="true" style="cursor:pointer" title="Send Mail"></i></a>
                                      
                  //               </div>`
                  //            //return null
                  //      }, "width": "5%"

                  //}
            ]

            columnDefs = [
                  //{
                  //      targets: [4],
                  //      "type": "date",
                  //      render: function (data, type, row) {
                  //            var insDate = data.split("T");
                  //            insDate = moment(insDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                  //            return insDate;
                  //      }
                  //},
                  {
                        targets: [6],
                        "type": "num",
                        render: function (data, type, row) {
                              if (data == null) {
                                    return ""
                              }
                              return data
                        }
                  },
                  {
                        targets: [7],
                        render: function (data, type, row) {
                              if (data == true) {
                                    return `<p class="badge bg-danger">YES</p>`
                              }
                              return `<p class="badge bg-success">No</p>`
                        }
                  },
                  //{
                  //      targets: [8],
                  //      "type": "date",
                  //      render: function (data, type, row) {
                  //            if (data != null) {
                  //                  var insDate = data.split("T");
                  //                  insDate = moment(insDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                  //                  return insDate;
                  //            }
                  //            return ""
                  //      }
                  //},
            ]

      }

      dataTable = $('#inspectionTableIdx').DataTable({
            "responsive": true,
            "lengthChange": true,
            "autoWidth": false,
            "searching": false,
            "deferRender": true,
            "bProcessing": true,
            "ajax": {
                  "url": "/GetAllInspectionReport?code=" + code,
                  "type": "POST",
                  "data": function (d) {
                        var formData = new FormData();
                        formData.append('Inspector', $('#insidx').val());
                        formData.append('SearchBy', $('#searchbyidx').val());
                        formData.append('FromDate', $('#lowerdaternge').val());
                        formData.append('ToDate', $('#upperdaternge').val());

                        var plainObject = {};
                        formData.forEach(function (value, key) {
                              plainObject[key] = value;
                        });
                        return plainObject;
                  }
            },
            "columns": columns,
            columnDefs: columnDefs,

            "language": {
                  "emptyTable": "No Records found"
            },
            "width": "100%",
      });
}


//function MailModal(url) {
//      $('#SendMail').trigger("reset");
//      //$('#IID').val(InspectionId);
//      $.ajax({
//            type: "GET",
//            url: url,
//            //data: InspectionId,
//            success: function (data) {
//                  if (data.success) {
//                        $('#ToMail').val(data.email);
//                        $('#IID').val(data.id);
//                        $('#purpose').val(data.purpose);
//                  }
//            },
//            complete: function () {
//                  $('#mailSend').modal('show');
//            }

//      })
//}

//function Modalert() {
//      var flg = 0;
//      //console.log($('#purpose').val());
//      if ($('#ToMail').val() != "" || $('#CCmail').val() != "" || $('#sub').val() != "" || $('#bdy').val() != "") {
//            flg = 1;
//      }
//      if (flg == 1) {
//            if (confirm("You have some unsaved Changes. On closing, your progress will be lost")) {
//                  $('#mailSend').modal('hide');
//            }
//      }
//      else {
//            $('#mailSend').modal('hide');
//      }
//}


//function SendMail() {
//      console.log($('#purpose').val());
//      let regex = new RegExp("([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\"\(\[\]!#-[^-~ \t]|(\\[\t -~]))+\")@([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\[[\t -Z^-~]*])");
//      var flg = 0;
//      if ($('#ToMail').val() == null || $('#ToMail').val() == "") {
//            $('#toEmailError').text("\u24d8 This Field is Required");
//            setTimeout(() => {
//                  $('#toEmailError').html("");
//            }, 5000)
//            flg = 1;
//      }
//      else {
//            var str = $('#ToMail').val();
//            var mailids = str.split(',');
//            for (let i = 0; i < mailids.length; i++) {
//                  if (regex.test(mailids[i]) == false) {
//                        $('#toEmailError').text("\u24d8 One or more Email Id is invalid");
//                        setTimeout(() => {
//                              $('#toEmailError').html("");
//                        }, 5000)
//                        flg = 1;
//                  }
//            }
//      }
//      if ($('#sub').val() == null || $('#sub').val() == "") {
//            $('#subError').text("\u24d8 This Field is Required");
//            setTimeout(() => {
//                  $('#subError').html("");
//            }, 5000)
//            flg = 1;
//      }
//      if (flg == 1) {
//            return false;
//      }

//      var purpose = $('#purpose').val();
//      var url = "";
//      if (purpose == "Opening Inspection") {
//            url = "/SendOpeningInsReportMailPdf";
//      }
//      else if (purpose == "Walk Through") {
//            url = "/SendWalkThroughInsReportMailPdf";
//      }
//      else {
//            url = "/SendInsReportMailPdf";
//      }
//      var data = $('#SendMail').serialize();
//      $.ajax({
//            type: "POST",
//            url: url,
//            data: data,
//            beforeSend: function () {
//                  $('div#loading-wrapper').hide();
//                  $('#spin').show();
//                  $('#saveicon').hide();
//                  $('#sendBtn').prop('disabled', true)
//            },
//            success: function (data) {
//                  //console.log(data);
//                  if (data.success) {

//                        $('audio#success_sound')[0].play();
//                        setTimeout(() => {
//                              toastr.info(data.msg);
//                        }, 500)
//                  }
//                  else {
//                        $('#mailSend').modal('hide');
//                        $('audio#errorsound')[0].play();
//                        setTimeout(() => {
//                              toastr.error(data.msg);
//                        }, 775)
//                  }
//                  $('div#overlay').hide();
//            },
//            complete: function () {
//                  $('div#loading-wrapper').hide();
//                  $('#spin').hide();
//                  $('#saveicon').show();
//                  $('#sendBtn').prop('disabled', false)
//                  $('#mailSend').modal('hide');
//            }
//      });
//}
