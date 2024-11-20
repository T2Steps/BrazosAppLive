var Inspectiontable;
function loadInspectionTable() {
    //alert($('#eid').val());
    var encryptedid = $('#encryptedEstId').val();
    var url = window.location.href;
    //alert(id);
    Schtable = $('#InspectionTable').DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "bProcessing": true,
        "ajax": {
            "url": "/GetAllIns?id=" + encryptedid

        },
        "columns": [
            {
                data: 'encryptedId',
                render: function (data, type, row, meta) {
                    return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                }
                , "width": "5%"
            },
            { "data": "purpose", "width": "10%" },
            { "data": "inspectionDate", "width": "10%" },
            { "data": "inspectedBy", "width": "10%" },
            { "data": "score", "width": "5%", className: "text-center" },
            { "data": "followUp", "width": "10%", className: "text-center" },
            { "data": "followUpDate", "width": "10%" },
            {
                "data": "encryptedId", "render": function (data, type, row, meta) {
                    var Pdfurl = "";
                    var downLoadBtn = '';
                    //console.log(row.purpose);
                    if (row.purpose == "Opening Inspection") {
                        Pdfurl = "/GetOpeningInspectionPdf"
                    }
                    else if (row.purpose == "Walk Through") {
                        Pdfurl = "/GetWalkThroughInspectionPdf"
                    }
                    else {
                        Pdfurl = "/GetInspectionPdf"
                        if (row.purpose != "Routine") {
                            downLoadBtn = `<a class="btn btn-sm btn-custom"  href="/DownloadInspectionCommentCertificatePdf?id=${data}" target="blank"><i class="fa-solid fa-download" aria-hidden="true" style="cursor:pointer" title="Download Comment Section"></i></a>`
                        }
                    }
                    if (!url.includes('View')) {
                        return `<div class="m-75 btn-group"  role="group">
                                      <a class="btn btn-sm btn-custom"  href="${Pdfurl}?id=${data}" target="blank"><i class="fas fa-file-pdf" aria-hidden="true" style="cursor:pointer" title="View Report"></i></a>
                                      ${downLoadBtn}
                                      <a class="btn btn-sm btn-custom" onclick="MailModal('/GetEstOwnerEmail?InspectionId=${data}')"><i class="fa-solid fa-envelope" aria-hidden="true" style="cursor:pointer" title="Send Mail"></i></a>
                                </div>`
                    }
                    else {
                        return `<div class="m-75 btn-group"  role="group">
                            <a class="btn btn-sm btn-custom"  href="${Pdfurl}?id=${data}" target="blank"><i class="fas fa-file-pdf" aria-hidden="true" style="cursor:pointer" title="View Report"></i></a>
                                      ${downLoadBtn}
                        </div>`
                    }
                    //return null
                }, "width": "5%"

            }
        ],
        columnDefs: [
            {
                targets: [2],
                render: function (data, type, row) {
                    var insDate = data.split("T");
                    insDate = moment(insDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                    return insDate;
                }
            },
            {
                targets: [4],
                "type": "num",
                render: function (data, type, row) {
                    if (data == null) {
                        return ''
                    }
                    else {
                          if (row.purpose == 'Routine') {
                                if (row.isPermitSuspended == true) {
                                      return 'PS'
                                }
                                else {
                                      return data
                                }
                          }
                          else {
                                if (row.isPermitSuspended == true) {
                                      return 'PS'
                                }
                                else {
                                      return 'NS'
                                }
                          }
                    }
                    
                }
            },
            {
                targets: [5],
                render: function (data, type, row) {
                    //var insDate = data.split("T");
                    //insDate = moment(insDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                    if (data == true) {
                        return `<p class="badge bg-danger">YES</p>`
                    }
                    return `<p class="badge bg-success">No</p>`
                }
            },
            {
                targets: [6],
                render: function (data, type, row) {
                    if (data != null) {
                        var insDate = data.split("T");
                        insDate = moment(insDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                        return insDate;
                    }
                    return "---"
                }
            },


        ],
        "language": {
            "emptyTable": "No Records found"
        },
        "width": "100%",
        "createdRow": function (row, data, dataIndex) {
            if ($(data)[0].isInspected == "Yes") {
                $(row).css('color', 'red');
            }
            if ($(data)[0].statusId == 3) {
                $(row).css('color', 'red');
            }
        }
    });
}

function MailModal(url) {
      $('#SendMail').trigger("reset");
      //$('#IID').val(InspectionId);
      $.ajax({
            type: "GET",
            url: url,
            //data: InspectionId,
            success: function (data) {
                  if (data.success) {
                        $('#ToMail').val(data.email);
                        $('#IID').val(data.id);
                        $('#insPurpose').val(data.purpose);

                  }
            },
            complete: function () {
                  $('#mailSend').modal('show');
            }

      })
}

function InsModalert() {
    var flg = 0;
    //console.log($('#purpose').val());
    if ($('#ToMail').val() != "" || $('#CCmail').val() != "" || $('#sub').val() != "" || $('#bdy').val() != "") {
        flg = 1;
    }
    if (flg == 1) {
        if (confirm("You have some unsaved Changes. On closing, your progress will be lost")) {
            $('#mailSend').modal('hide');
        }
    }
    else {
        $('#mailSend').modal('hide');
    }
}


function SendMail() {
    
    let regex = new RegExp("([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\"\(\[\]!#-[^-~ \t]|(\\[\t -~]))+\")@([!#-'*+/-9=?A-Z^-~-]+(\.[!#-'*+/-9=?A-Z^-~-]+)*|\[[\t -Z^-~]*])");
    var flg = 0;
    if ($('#ToMail').val() == null || $('#ToMail').val() == "") {
        $('#toEmailError').text("\u24d8 This Field is Required");
        setTimeout(() => {
            $('#toEmailError').html("");
        }, 5000)
        flg = 1;
    }
    else {
        var str = $('#ToMail').val();
        var mailids = str.split(',');
        for (let i = 0; i < mailids.length; i++) {
            if (regex.test(mailids[i]) == false) {
                $('#toEmailError').text("\u24d8 One or more Email Id is invalid");
                setTimeout(() => {
                    $('#toEmailError').html("");
                }, 5000)
                flg = 1;
            }
        }
    }
    if ($('#sub').val() == null || $('#sub').val() == "") {
        $('#subError').text("\u24d8 This Field is Required");
        setTimeout(() => {
            $('#subError').html("");
        }, 5000)
        flg = 1;
    }
    if (flg == 1) {
        return false;
    }

    var purpose = $('#insPurpose').val();
    //console.log(purpose)
    var url = "";
    if (purpose == "Opening Inspection") {
        url = "/SendOpeningInsReportMailPdf";
    }
    else if (purpose == "Walk Through") {
        url = "/SendWalkThroughInsReportMailPdf";
    }
    else {
        url = "/SendInsReportMailPdf";
    }
    var data = $('#SendMail').serialize();
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        beforeSend: function () {
            $('div#loading-wrapper').show();
            $('#spin').show();
            $('#saveicon').hide();
            $('#sendBtn').prop('disabled', true)
        },
        success: function (data) {
            //console.log(data);
            if (data.success) {

                $('audio#success_sound')[0].play();
                setTimeout(() => {
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
            //$('div#overlay').hide();
        },
        complete: function () {
            $('div#loading-wrapper').hide();
            $('#spin').hide();
            $('#saveicon').show();
            $('#sendBtn').prop('disabled', false)
            $('#mailSend').modal('hide');
        }
    });
}