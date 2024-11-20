var dataTable;
var dataTableRetail;
var dataTableMobile;
var dataTableTemp;

$(function () {
    loadDataTable();
    
});

function loadDataTable() {

    dataTable = $('#permitIdx').DataTable({
        "responsive": true,
        "lengthChange": false,
        "autoWidth": false,
        "bProcessing": true,
        "ajax": {
            "url": "/GetAllPermits"
        },
        "columns": [
            {
                data: 'id',
                render: function (data, type, row, meta) {
                    //console.log(row);
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { "data": "permitNumber" },
            { "data": "name" },
            
            { "data": "territory" },
            { "data": "riskCategory" },
            { "data": "permitStatus" },
            { "data": "applicationFor" },
            { "data": "activationDate" },
            { "data": "expiryDate" },
            /*{ "data": "certifiedPoolOperator" },*/

            {
                "data": "encryptedId", "render": function (data, type, row, meta) {
                    //console.log(row);
                    console.log(row.isActive);
                    if (row.isActive == true) {
                        if (row.code == "RF")
                        {
                            return `<div class="m-75 btn-group"  role="group">      
                                          <a class="btn btn-sm btn-custom"  href="/RFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
                                    </div>`
                        }
                        if (row.code == "MF") {
                            return `<div class="m-75 btn-group"  role="group">      
                                          <a class="btn btn-sm btn-custom"  href="/MFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
                                    </div>`
                        }
                    }
                    else {
                        return `<div class="m-75 btn-group"  role="group">      
                                          <a class="btn btn-sm btn-custom"  onclick=ActiveInactive('/ChangeUserState?id=${data}')><i class="fa-solid fa-check" id="icon"  style="cursor:pointer" title="Active"></i> </a>
                                    </div>`
                    }
                }

            }

        ],
        columnDefs: [
            {
                targets: [3],
                render: function (data, type, row) {
                    if (data == null) {
                        return "NA"
                    }
                    return data;
                }
            },
            {
                targets: [4],
                render: function (data, type, row) {
                    if (data == null) {
                        return "NA"
                    }
                    return data;
                }
            },
            {
                targets: [5],
                      render: function (data, type, row) {
                            if (data == "Incomplete" || data == "Pending Admin Review" || data == "Admin Review" || data == "Pending Plan Review" || data == "Plan Review" || data == "Pending Build-Out" || data == "Pending Payment" || data == "Opening Inspection") {
                                  return `<p class="badge bg-light">${data}</p>`
                            }
                            else if (data == "Active") {
                                  return `<p class="badge bg-success">${data}</p>`
                            }
                            else if (data == "Renewal") {
                                  return `<p class="badge bg-warning">${data}</p>`
                            }
                            else if (data == "Expired") {
                                  return `<p class="badge bg-danger">${data}</p>`
                            }
                            else if (data == "Expired") {
                                  return `<p class="badge bg-danger">${data}</p>`
                            }
                            else {
                                  return `<p class="badge bg-secondary">${data}</p>`
                            }
                    //else if (data == "Complaint") {
                    //    return `<p class="badge bg-danger">Complaint</p>`
                    //}
                }
            },
            {
                targets: [6],
                render: function (data, type, row) {
                    if (data == "NewPermit") {
                        return `<p class="badge bg-success">New Permit</p>`
                    }
                    else if (data == "OwnerChange") {
                        return `<p class="badge bg-warning">Owner Change</p>`
                    }
                    else if (data == "Complaint") {
                        return `<p class="badge bg-danger">Complaint</p>`
                    }
                }
            },
            {
                targets: [7],
                render: function (data, type, row) {
                    if (data == null) {
                        return "--"
                    }
                    else {
                        var actDate = data.split("T");
                        actDate = moment(insDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                        return actDate;
                    }
                    
                }
            },
            {
                targets: [8],
                render: function (data, type, row) {
                    if (data == null) {
                        return "--"
                    }
                    else {
                        var expDate = data.split("T");
                        expDate = moment(expDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                        return expDate;
                    }
                }
            },
        ],
        "language": {
            "emptyTable": "No Records found",
            "processing":
                '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span> '
        },
        "width": "100%",
        "createdRow": function (row, data, dataIndex) {
            if ($(data)[0].isActive == false) {
                $(row).css('color', 'red');
            }
        }
    });
}

function loadRetailDataTable(Role) {
    dataTableRetail = $('#retailPermitIdx').DataTable({
        
        "responsive": true,
        "lengthChange": true,
        "autoWidth": true,
        "searching": false,
        "scrollX": true,
        "deferRender": true,
        "bProcessing": true,
        "pageLength" : 50,
        "aaSorting": [],
        "ajax": {
            "url": "/GetAllRetailPermits",
            "type": "POST", 
            "data": function (d) {
                var formData = new FormData();
                formData.append('Name', $('#estName').val());
                formData.append('Permit', $('#permitNo').val());
                formData.append('Owner', $('#owner').val());
                formData.append('Address', $('#estAddress').val());
                formData.append('City', $('#estCity').val());
                formData.append('EstType', $('#estType').val());
                formData.append('ApplicationNo', $('#applicationNo').val());
                formData.append('Area', $('#areaNo').val());
                formData.append('Risk', $('#riskidx').val());
                formData.append('PermitStatus', $('#pStat').val());
                formData.append('SearchBy', $('#searchby').val());
                //formData.append('Purpose', $('#purposeidx').val());
                formData.append('FromDate', $('#lowerdaternge').val());
                formData.append('ToDate', $('#upperdaternge').val());

                var plainObject = {};
                formData.forEach(function (value, key) {
                    plainObject[key] = value;
                });
                return plainObject;
            }
            
        },
        "columns": [
            //{
                //data: 'id',
                //render: function (data, type, row, meta) {
                    //console.log(meta.row);
                    //console.log(meta.settings._iDisplayStart);
                    //return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                //}, "width": "2%"
            //},
            { "data": "applicationNumber" },
            //{ "data": "applicationDate", "width": "8%" },
            { "data": "permitNumber" },
            { "data": "name" },
            { "data": "owner" },
            { "data": "address"},
            { "data": "city" },
            { "data": "category" },
            { "data": "area"},
            { "data": "riskCategory" },
            { "data": "permitStatus" },
            { "data": "lastinspectionDate" },
            { "data": "nextinspectionDate" },
            { "data": "activationDate" },
            { "data": "expiryDate" },
            /*{ "data": "certifiedPoolOperator" },*/
            {
                "data": "encryptedId", "render": function (data, type, row, meta) {
                    var editBtn = `<a class="btn btn-sm btn-custom" href="/RFEdit?id=${data}">
                                <i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i>
                            </a>`;
                    var viewBtn = `<a class="btn btn-sm btn-custom" href="/RFPermitView?id=${data}">
                                <i class="fa-solid fa-eye" aria-hidden="true" style="cursor:pointer" title="View"></i>
                            </a>`;
                    var downLoadCertificateBtn = `<a class="btn btn-sm btn-custom" href="/DownloadPermitCertificatePdf?id=${data}">
                                <i class="fa-solid fa-download" aria-hidden="true" style="cursor:pointer" title="Download Permit Certificate"></i>
                            </a>`;
                    var InactiveBtn = `<a class="btn btn-sm btn-danger" onclick="ActiveInactivePermit('/ActiveInactivePermit?id=${data}', '#retailPermitIdx', 'Inactivate', '${row.permitNumber}')">
                                <i class="fas fa-trash" id="icon" style="cursor:pointer" title="Inactive Permit"></i>
                            </a>`
                    var ActiveBtn = `<a class="btn btn-sm btn-custom" onclick="ActiveInactivePermit('/ActiveInactivePermit?id=${data}', '#retailPermitIdx', 'Reactivate', '${row.permitNumber}')">
                                            <i class="fa-solid fa-check" id="icon" style="cursor:pointer" title="Reactivate Permit"></i>
                                        </a>`
                    var closeBtn = `<a class="btn btn-sm btn-danger" onclick="ClosePermit('/ClosePermit?id=${data}', '#retailPermitIdx', '${row.permitNumber}')">
                                        <i class="fa-solid fa-circle-xmark" id="icon" style="cursor:pointer" title="Close Permit"></i>
                                    </a>`
                    //var renewBtn = `<a class="btn btn-sm btn-warning" href="/RFEdit?id=${data}">
                    //            <i class="fa-solid fa-rotate" aria-hidden="true" style="cursor:pointer" title="Renew"></i>
                    //        </a>`
                    var renewBtn = `<a class="btn btn-sm btn-warning" onclick="GoToRenewPage('/RFEdit?id=${data}', '${row.permitNumber}')">
                                <i class="fa-solid fa-rotate" aria-hidden="true" style="cursor:pointer" title="Renew"></i>
                            </a>`
                    var mailBtn = `<a class="btn btn-sm btn-custom" onclick="MailModal('/GetCertificateEstOwnerEmail?EstId=${data}')"><i class="fa-solid fa-envelope" aria-hidden="true" style="cursor:pointer" title="Send Mail"></i></a>`
                    if (Role != "SuperAdmin" && Role != "Admin" && Role != "Admin Inspector")
                    {
                        closeBtn = ``;
                    }
                    //href = "/Renewal?id=${data}"
                    if (row.permitStatus == "Active") {
                        return `
                        <div class="m-75 btn-group" role="group">
                            ${viewBtn}
                            ${editBtn}
                            ${downLoadCertificateBtn}
                            ${mailBtn}
                            ${InactiveBtn}
                        </div>`

                    }
                    if (row.permitStatus == "Renewal") {
                        return `
                        <div class="m-75 btn-group" role="group">
                            ${viewBtn}
                            ${editBtn}
                            ${downLoadCertificateBtn}
                            ${mailBtn}
                            ${renewBtn}
                            ${InactiveBtn}  
                        </div>`
                        //${renewBtn}
                        //${InactiveBtn}  
                    }
                    if (row.permitStatus == "Expired") {
                        return `
                        <div class="m-75 btn-group" role="group">
                            ${viewBtn}
                            ${editBtn}
                            ${renewBtn}
                            ${InactiveBtn}
                        </div>`
                        //${renewBtn}
                    }
                    if (row.permitStatus == "Area 51") {
                        return `
                        <div class="m-75 btn-group" role="group">
                            ${viewBtn}
                            ${editBtn}
                            ${renewBtn}
                            ${InactiveBtn}
                        </div>`
                        //${renewBtn}
                    }
                    if (row.permitStatus == "Inactive") {
                        return `
                        <div class="m-75 btn-group" role="group">
                            ${viewBtn}
                            ${ActiveBtn}
                            ${renewBtn}
                            ${closeBtn}
                        </div>`
                        //${renewBtn}
                    }
                    if (row.permitStatus == "Closed") {
                        return `<div class="m-75 btn-group"  role="group">  
                                       ${viewBtn}
                                      
                                </div>`
                    }
                    else {
                          if (row.permitStatus == "Pending Payment" || row.permitStatus == "Opening Inspection") {
                                return `<div class="m-75 btn-group"  role="group"> 
                                          ${viewBtn}
                                          ${editBtn}
                                          ${downLoadCertificateBtn}
                                          ${mailBtn}
                                </div>`
                          }
                          else {
                              return `<div class="m-75 btn-group"  role="group">  
                                       ${viewBtn}
                                      <a class="btn btn-sm btn-custom"  href="/RFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
                                </div>`
                          }
                        
                    }
                }
            }
        ],
        columnDefs: [
            //{
                //targets: [2],
                //render: function (data, type, row) {
                    //if (data == null) {
                        //return "--"
                    //}
                   // else {
                        //var appDate = data.split("T");
                       // appDate = moment(appDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                       // return appDate;
                    //}
                //}
            //},
            {
                targets: [1],
                render: function (data, type, row) {
                    if (data == null) {
                        return "--"
                    }
                    return data;
                }
            },
            {
                targets: [6],
                render: function (data, type, row) {
                    if (data == null) {
                        return "--"
                    }
                    return data;
                }
            },
            {
                targets: [7],
                render: function (data, type, row) {
                    if (data == 0) {
                        return "Unassigned"
                    }
                    return data;
                }
            },
            {
                targets: [8],
                render: function (data, type, row) {
                    if (data == null) {
                        return "NA"
                    }
                    return data;
                }
            },
            {
                targets: [9],
                render: function (data, type, row) {
                    if (data == "Incomplete" || data == "Pending Admin Review" || data == "Admin Review" || data == "Pending Plan Review" || data == "Plan Review" || data == "Pending Build-Out" || data == "Pending Payment" || data == "Opening Inspection") {
                        return `<p class="badge bg-light">${data}</p>`
                    }
                    else if (data == "Active") {
                        return `<p class="badge bg-success">${data}</p>`
                    }
                    else if (data == "Renewal") {
                        return `<p class="badge bg-warning">${data}</p>`
                    }
                    else if (data == "Expired") {
                        return `<p class="badge bg-danger">${data}</p>`
                    }
                    else {
                        return `<p class="badge bg-secondary">${data}</p>`
                    }
                    //else if (data == "Complaint") {
                    //    return `<p class="badge bg-danger">Complaint</p>`
                    //}
                }
            },
            {
                targets: [10],
                "type": "date",
                render: function (data, type, row) {
                    if (data == null) {
                        return ""
                    }
                    else {
                        var lastrinsDate = data.split("T");
                        lastrinsDate = moment(lastrinsDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                        return lastrinsDate;
                        
                    }
                }
            },
            {
                targets: [11],
                "type": "date",
                render: function (data, type, row) {
                    if (data == null) {
                        return ""
                    }
                    else {
                        var insDate = data.split("T");
                        insDate = moment(insDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                        return insDate;

                    }
                }
            },
            {
                targets: [12],
                "type": "date",
                render: function (data, type, row) {
                    if (data == null) {
                        return ""
                    }
                    else {
                        var actDate = data.split("T");
                        actDate = moment(actDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                        return actDate;
                    }
                }
            },
            {
                targets: [13],
                "type": "date",
                render: function (data, type, row) {
                    if (data == null) {
                        return ""
                    }
                    else {
                        var expDate = data.split("T");
                        expDate = moment(expDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                        return expDate;
                    }
                }
            },
        ],
        "language": {
            "emptyTable": "No records found",
            //"processing":
            //    '<i class="fa fa-spinner fa-spin fa-3x fa-fw" style="color:#2a2b2b;"></i><span class="sr-only">Loading...</span> '
        },
        "width": "100%",
        "createdRow": function (row, data, dataIndex) {
            if ($(data)[0].isActive == false) {
                $(row).css('color', 'red');
            }
        }
    });

    //$('#txtFromDate, #txtToDate').on('change', function () {
    //    $('#tblFoodInspectionList').DataTable().draw();
    //});

      //$('#retailPermitIdx thead tr:eq(0) th').each(function (i) {
      //      $('input', this).on('keyup change clear', function () {
      //            //console.log(this.value);
      //            //console.log(i);
      //            if (dataTableRetail.column(i).search() !== this.value) {
      //                  dataTableRetail
      //                        .column(i)
      //                        .search(this.value)
      //                        .draw();
      //            }
      //      });
      //});

      //$('#retailPermitIdx thead tr:eq(0) th').each(function (i) {
      //      const input = $('input', this);
      //      if (input.attr('type') === 'date') {
      //            input.on('change', function () {
      //                  const searchVal = this.value;
      //                  const formattedVal = searchVal ? moment(searchVal, "YYYY-MM-DD").format("MM/DD/YYYY") : '';
      //                  dataTableRetail
      //                        .column(i)
      //                        .search(formattedVal)
      //                        .draw();
      //            });
      //      } else {
      //            input.on('keyup change clear', function () {
      //                  if (dataTableRetail.column(i).search() !== this.value) {
      //                        dataTableRetail
      //                              .column(i)
      //                              .search(this.value)
      //                              .draw();
      //                  }
      //            });
      //      }
      //});

}

function loadMobileDataTable(Role) {

    dataTableMobile = $('#mobilePermitIdx').DataTable({
        "responsive": true,
        "lengthChange": true,
        "autoWidth": true,
        "searching": false,
        "scrollX": true,
        "deferRender": true,
        "bProcessing": true,
        "pageLength": 50,
        "aaSorting": [],
        "ajax": {
            "url": "/GetAllMobilePermits",
            "type": "POST",
            "data": function (d) {
                //var formData = new FormData();
                //formData.append('Name', $('#estName').val());
                //formData.append('Permit', $('#permitNo').val());
                //formData.append('ApplicationNo', $('#applicationNo').val());
                //formData.append('Area', $('#areaNo').val());
                //formData.append('Risk', $('#riskidx').val());
                //formData.append('PermitStatus', $('#pStat').val());
                ////formData.append('Purpose', $('#purposeidx').val());
                //formData.append('FromDate', $('#lowerdaternge').val());
                //formData.append('ToDate', $('#upperdaternge').val());
                var formData = new FormData();
                formData.append('Name', $('#estName').val());
                formData.append('Permit', $('#permitNo').val());
                formData.append('Owner', $('#owner').val());
                formData.append('Address', $('#estAddress').val());
                formData.append('City', $('#estCity').val());
                formData.append('EstType', $('#estType').val());
                formData.append('ApplicationNo', $('#applicationNo').val());
                formData.append('Area', $('#areaNo').val());
                formData.append('Risk', $('#riskidx').val());
                formData.append('PermitStatus', $('#pStat').val());
                formData.append('SearchBy', $('#searchby').val());
                //formData.append('Purpose', $('#purposeidx').val());
                formData.append('FromDate', $('#lowerdaternge').val());
                formData.append('ToDate', $('#upperdaternge').val());

                var plainObject = {};
                formData.forEach(function (value, key) {
                    plainObject[key] = value;
                });
                return plainObject;
            }
        },
        "columns": [
            //{
                //data: 'id',
                //render: function (data, type, row, meta) {
                    //console.log(row);
                   // return meta.row + /*meta.settings._iDisplayStart +*/ 1;
                //}, "width": "2%"
            //},
            //{ "data": "applicationNumber", "width": "10%" },
            ////{ "data": "applicationDate", "width": "8%" },
            //{ "data": "permitNumber", "width": "10%" },
            //{ "data": "name", "width": "20%" },
            //{ "data": "area", "width": "8%" },
            //{ "data": "riskCategory", "width": "8%" },
            //{ "data": "permitStatus", "width": "8%" },
            //{ "data": "createdBy", "width": "8%" },
            //{ "data": "activationDate", "width": "8%" },
            //{ "data": "expiryDate", "width": "8%" },
            /*{ "data": "certifiedPoolOperator" },*/
            { "data": "applicationNumber" },
            //{ "data": "applicationDate", "width": "8%" },
            { "data": "permitNumber" },
            { "data": "name" },
            { "data": "owner" },
            { "data": "address" },
            { "data": "city" },
            { "data": "category" },
            { "data": "area" },
            { "data": "riskCategory" },
            { "data": "permitStatus" },
            { "data": "lastinspectionDate" },
            { "data": "nextinspectionDate" },
            { "data": "activationDate" },
            { "data": "expiryDate" },
            {
                "data": "encryptedId", "render": function (data, type, row, meta) {
                    var editBtn = `<a class="btn btn-sm btn-custom" href="/MFEdit?id=${data}">
                                <i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i>
                            </a>`;
                    var downLoadCertificateBtn = `<a class="btn btn-sm btn-custom" href="/DownloadPermitCertificatePdf?id=${data}">
                                <i class="fa-solid fa-download" aria-hidden="true" style="cursor:pointer" title="Download Permit Certificate"></i>
                            </a>`;
                    var InactiveBtn = `<a class="btn btn-sm btn-danger" onclick="ActiveInactivePermit('/ActiveInactivePermit?id=${data}', '#mobilePermitIdx', 'Inactivate', '${row.permitNumber}')">
                                <i class="fas fa-trash" id="icon" style="cursor:pointer" title="Inactive Permit"></i>
                            </a>`
                    var ActiveBtn = `<a class="btn btn-sm btn-custom" onclick="ActiveInactivePermit('/ActiveInactivePermit?id=${data}', '#mobilePermitIdx', 'Reactivate', '${row.permitNumber}')">
                                            <i class="fa-solid fa-check" id="icon" style="cursor:pointer" title="Reactivate Permit"></i>
                                        </a>`
                    var closeBtn = `<a class="btn btn-sm btn-danger" onclick="ClosePermit('/ClosePermit?id=${data}', '#mobilePermitIdx', '${row.permitNumber}')">
                                        <i class="fa-solid fa-circle-xmark" id="icon" style="cursor:pointer" title="Close Permit"></i>
                                    </a>`
                    //var renewBtn = `<a class="btn btn-sm btn-warning" href="/MFEdit?id=${data}">
                    //            <i class="fa-solid fa-rotate" aria-hidden="true" style="cursor:pointer" title="Renew"></i>
                    //        </a>`
                    var renewBtn = `<a class="btn btn-sm btn-warning" onclick="GoToRenewPage('/MFEdit?id=${data}', '${row.permitNumber}')">
                                <i class="fa-solid fa-rotate" aria-hidden="true" style="cursor:pointer" title="Renew"></i>
                            </a>`
                    var mailBtn = `<a class="btn btn-sm btn-custom" onclick="MailModal('/GetCertificateEstOwnerEmail?EstId=${data}')"><i class="fa-solid fa-envelope" aria-hidden="true" style="cursor:pointer" title="Send Mail"></i></a>`

                    if (Role != "SuperAdmin" && Role != "Admin" && Role != "Admin Inspector") {
                        closeBtn = ``;
                    }
                    //href = "/Renewal?id=${data}"
                    if (row.permitStatus == "Active") {
                        return `
                        <div class="m-75 btn-group" role="group">
                            ${editBtn}
                            ${downLoadCertificateBtn}
                            ${mailBtn}
                            ${InactiveBtn}
                        </div>`

                    }
                    if (row.permitStatus == "Renewal") {
                        return `
                        <div class="m-75 btn-group" role="group">      
                            ${editBtn}
                            ${downLoadCertificateBtn}
                            ${mailBtn}
                            ${renewBtn}
                            ${InactiveBtn}     
                        </div>`
                        //${renewBtn}
                    }
                    if (row.permitStatus == "Expired") {
                        return `
                        <div class="m-75 btn-group" role="group">      
                            ${editBtn}
                            ${renewBtn}
                            ${InactiveBtn}
                        </div>`
                        //${renewBtn}
                    }
                    if (row.permitStatus == "Area 51") {
                        return `
                        <div class="m-75 btn-group" role="group">      
                            ${editBtn}
                            ${renewBtn}
                            ${InactiveBtn}
                        </div>`
                        //${renewBtn}
                    }
                    if (row.permitStatus == "Inactive") {
                        return `
                        <div class="m-75 btn-group" role="group">      
                            ${ActiveBtn}
                            ${renewBtn}
                            ${closeBtn}
                        </div>`
                        //${renewBtn}
                    }
                    if (row.permitStatus == "Closed") {
                        return null
                    }
                    else {
                        if (row.permitStatus == "Pending Payment" || row.permitStatus == "Opening Inspection") {
                            return `<div class="m-75 btn-group"  role="group">      
                                          ${editBtn}
                                          ${downLoadCertificateBtn}
                                          ${mailBtn}
                                </div>`
                        }
                        else {
                            return `<div class="m-75 btn-group"  role="group">      
                                      <a class="btn btn-sm btn-custom"  href="/MFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
                                </div>`
                        }

                    }
                }
            }
            //{
            //    "data": "encryptedId", "render": function (data, type, row, meta) {
            //        if (row.isActive == true) {
            //            if (Role == "SuperAdmin" || Role == "Admin" || Role == "Admin Inspector") {
            //                if (row.code == "MF") {
            //                    if (row.permitStatus == "Active") {
            //                        return `
            //                            <div class="m-75 btn-group" role="group">      
            //                                <a class="btn btn-sm btn-custom" href="/MFEdit?id=${data}">
            //                                    <i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i>
            //                                </a>
            //                                <a class="btn btn-sm btn-custom" href="/DownloadPermitCertificatePdf?id=${data}">
            //                                    <i class="fa-solid fa-download" aria-hidden="true" style="cursor:pointer" title="Download Permit Certificate"></i>
            //                                </a>
            //                                <a class="btn btn-sm btn-danger" onclick="ActiveInactivePermit('/ActiveInactivePermit?id=${data}', '#mobilePermitIdx')">
            //                                    <i class="fas fa-trash" id="icon" style="cursor:pointer" title="Inactive Permit"></i>
            //                                </a>
            //                            </div>`

            //                    }
            //                    else {
            //                        return `<div class="m-75 btn-group"  role="group">      
            //                              <a class="btn btn-sm btn-custom"  href="/MFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
            //                        </div>`
            //                    }
            //                }
            //            }
            //            else if (Role == "Inspector") {
            //                if (row.code == "MF") {
            //                    if (row.permitStatus == "Active") {
            //                        return `<div class="m-75 btn-group"  role="group">      
            //                              <a class="btn btn-sm btn-custom"  href="/MFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
            //                              <a class="btn btn-sm btn-danger" onclick="ActiveInactivePermit('/ActiveInactivePermit?id=${data}', '#mobilePermitIdx')">
            //                                    <i class="fas fa-trash" id="icon" style="cursor:pointer" title="Inactive Permit"></i>
            //                                </a>
            //                        </div>`
            //                    }
            //                    else {
            //                        return `<div class="m-75 btn-group"  role="group">      
            //                              <a class="btn btn-sm btn-custom"  href="/MFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
            //                        </div>`
            //                    }

            //                }
            //            }
            //            else if (Role == "Clerk") {
            //                if (row.code == "MF") {
            //                    if (row.permitStatus == "Active") {
            //                        return `<div class="m-75 btn-group"  role="group">      
            //                                  <a class="btn btn-sm btn-custom"  href="/MFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
            //                                  <a class="btn btn-sm btn-danger" onclick="ActiveInactivePermit('/ActiveInactivePermit?id=${data}', '#mobilePermitIdx')">
            //                                    <i class="fas fa-trash" id="icon" style="cursor:pointer" title="Inactive Permit"></i>
            //                                </a>
            //                            </div>`
            //                    }
            //                    else {
            //                        return `<div class="m-75 btn-group"  role="group">      
            //                                  <a class="btn btn-sm btn-custom"  href="/MFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
            //                            </div>`
            //                    }
            //                }
            //            }
            //            else {
            //                return null
            //            }
            //        }
            //        else {
            //            if(row.permitStatus!="Closed"){
            //                var closeBtn = '';
            //                if (Role == "SuperAdmin" || Role == "Admin" || Role == "Admin Inspector") {
            //                        closeBtn = `<a class="btn btn-sm btn-danger" onclick="ClosePermit('/ClosePermit?id=${data}', '#mobilePermitIdx')">
            //                                                <i class="fa-solid fa-circle-xmark" id="icon" style="cursor:pointer" title="Close Permit"></i>
            //                                            </a>`   
            //                }
            //                return `<div class="m-75 btn-group"  role="group">
            //                        <a class="btn btn-sm btn-custom" onclick="ActiveInactivePermit('/ActiveInactivePermit?id=${data}', '#mobilePermitIdx')">
            //                                                <i class="fa-solid fa-check" id="icon" style="cursor:pointer" title="Reactivate Permit"></i>
            //                                            </a>
            //                                          ${closeBtn}
            //                                    </div>`
            //            }
            //            else{
            //                return null;
            //            }
                        
            //        }
            //    }
            //}
        ],
        "columnDefs": [
            {
                targets: [1],
                render: function (data, type, row) {
                    if (data == null) {
                        return "--"
                    }
                    return data;
                }
            },
            {
                targets: [6],
                render: function (data, type, row) {
                    if (data == null) {
                        return "--"
                    }
                    return data;
                }
            },
            {
                targets: [7],
                render: function (data, type, row) {
                    if (data == 0) {
                        return "Unassigned"
                    }
                    return data;
                }
            },
            {
                targets: [8],
                render: function (data, type, row) {
                    if (data == null) {
                        return "NA"
                    }
                    return data;
                }
            },
            {
                targets: [9],
                render: function (data, type, row) {
                    if (data == "Incomplete" || data == "Pending Admin Review" || data == "Admin Review" || data == "Pending Plan Review" || data == "Plan Review" || data == "Pending Build-Out" || data == "Pending Payment" || data == "Opening Inspection") {
                        return `<p class="badge bg-light">${data}</p>`
                    }
                    else if (data == "Active") {
                        return `<p class="badge bg-success">${data}</p>`
                    }
                    else if (data == "Renewal") {
                        return `<p class="badge bg-warning">${data}</p>`
                    }
                    else if (data == "Expired") {
                        return `<p class="badge bg-danger">${data}</p>`
                    }
                    else {
                        return `<p class="badge bg-secondary">${data}</p>`
                    }
                    //else if (data == "Complaint") {
                    //    return `<p class="badge bg-danger">Complaint</p>`
                    //}
                }
            },
            {
                targets: [10],
                "type": "date",
                render: function (data, type, row) {
                    if (data == null) {
                        return ""
                    }
                    else {
                        var lastrinsDate = data.split("T");
                        lastrinsDate = moment(lastrinsDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                        return lastrinsDate;

                    }
                }
            },
            {
                targets: [11],
                "type": "date",
                render: function (data, type, row) {
                    if (data == null) {
                        return ""
                    }
                    else {
                        var insDate = data.split("T");
                        insDate = moment(insDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                        return insDate;

                    }
                }
            },
            {
                targets: [12],
                "type": "date",
                render: function (data, type, row) {
                    if (data == null) {
                        return ""
                    }
                    else {
                        var actDate = data.split("T");
                        actDate = moment(actDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                        return actDate;
                    }
                }
            },
            {
                targets: [13],
                "type": "date",
                render: function (data, type, row) {
                    if (data == null) {
                        return ""
                    }
                    else {
                        var expDate = data.split("T");
                        expDate = moment(expDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                        return expDate;
                    }
                }
            },


        ],
        //columnDefs: [
        //    //{
        //        //targets: [2],
        //        //render: function (data, type, row) {
        //            //if (data == null) {
        //                //return "--"
        //            //}
        //            //else {
        //                //var appDate = data.split("T");
        //                //appDate = moment(appDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
        //                //return appDate;
        //            //}
        //       // }
        //    //},
        //    {
        //        targets: [1],
        //        render: function (data, type, row) {
        //            if (data == null) {
        //                return "--"
        //            }
        //            return data;
        //        }
        //    },
        //    {
        //        targets: [3],
        //        render: function (data, type, row) {
        //            if (data == null) {
        //                return "NA"
        //            }
        //            return data;
        //        }
        //    },
        //    {
        //        targets: [4],
        //        render: function (data, type, row) {
        //            if (data == null) {
        //                return "NA"
        //            }
        //            return data;
        //        }
        //    },
        //    {
        //        targets: [5],
        //        render: function (data, type, row) {
        //            if (data == "Incomplete" || data == "Pending Admin Review" || data == "Admin Review" || data == "Pending Plan Review" || data == "Plan Review" || data == "Pending Build-Out" || data == "Pending Payment" || data == "Opening Inspection") {
        //                return `<p class="badge bg-light">${data}</p>`
        //            }
        //            else if (data == "Active") {
        //                return `<p class="badge bg-success">${data}</p>`
        //            }
        //            else if (data == "Renewal") {
        //                return `<p class="badge bg-warning">${data}</p>`
        //            }
        //            else if (data == "Expired") {
        //                return `<p class="badge bg-danger">${data}</p>`
        //            }
        //            else if (data == "Expired") {
        //                return `<p class="badge bg-danger">${data}</p>`
        //            }
        //            else {
        //                return `<p class="badge bg-secondary">${data}</p>`
        //            }
        //            //else if (data == "Complaint") {
        //            //    return `<p class="badge bg-danger">Complaint</p>`
        //            //}
        //        }
        //    },
        //    //{
        //    //    targets: [6],
        //    //    render: function (data, type, row) {
        //    //        if (data == "NewPermit") {
        //    //            return `<p class="badge bg-success">New Permit</p>`
        //    //        }
        //    //        else if (data == "OwnerChange") {
        //    //            return `<p class="badge bg-warning">Owner Change</p>`
        //    //        }
        //    //        else if (data == "Complaint") {
        //    //            return `<p class="badge bg-danger">Complaint</p>`
        //    //        }
        //    //    }
        //    //},
        //    {
        //        targets: [6],
        //        render: function (data, type, row) {
        //            if (data == " ") {
        //                return "Applicant"
        //            }
        //            else {
        //                return data;
        //            }
        //        }
        //    },
        //    {
        //        targets: [7],
        //        render: function (data, type, row) {
        //            if (data == null) {
        //                return "--"
        //            }
        //            else {
        //                var actDate = data.split("T");
        //                actDate = moment(actDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
        //                return actDate;
        //            }
        //        }
        //    },
        //    {
        //        targets: [8],
        //        render: function (data, type, row) {
        //            if (data == null) {
        //                return "--"
        //            }
        //            else {
        //                var expDate = data.split("T");
        //                expDate = moment(expDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
        //                return expDate;
        //            }
        //        }
        //    },
        //],
        "language": {
            "emptyTable": "No records found"
        },
        "width": "100%",
        "createdRow": function (row, data, dataIndex) {
            if ($(data)[0].isActive == false) {
                $(row).css('color', 'red');
            }
        }
    });

      $('#mobilePermitIdx thead tr:eq(0) th').each(function (i) {
            $('input', this).on('keyup change clear', function () {
                  if (dataTableMobile.column(i).search() !== this.value) {
                        dataTableMobile
                              .column(i)
                              .search(this.value)
                              .draw();
                  }
            });
      });
}

function loadTempDataTable(Role) {

    dataTableTemp = $('#tempPermitIdx').DataTable({
        "responsive": true,
        "lengthChange": true,
        "autoWidth": true,
        "searching": false,
        "scrollX": true,
        "deferRender": true,
        "bProcessing": true,
        "pageLength": 50,
        "aaSorting": [],
        "ajax": {
            "url": "/GetAllTempPermits",
            "type": "POST",
            "data": function (d) {
                var formData = new FormData();
                formData.append('Name', $('#estName').val());
                formData.append('Permit', $('#permitNo').val());
                formData.append('Owner', $('#owner').val());
                formData.append('Address', $('#estAddress').val());
                formData.append('City', $('#estCity').val());
                formData.append('ApplicationNo', $('#applicationNo').val());
                formData.append('PermitStatus', $('#pStat').val());
                formData.append('SearchBy', $('#searchby').val());
                formData.append('FromDate', $('#lowerdaternge').val());
                formData.append('ToDate', $('#upperdaternge').val());

                var plainObject = {};
                formData.forEach(function (value, key) {
                    plainObject[key] = value;
                });
                return plainObject;
            }
        },
        "columns": [
              { "data": "applicationNumber" },
              { "data": "applicationDate" },
              { "data": "permitNumber" },
              { "data": "events" },
              { "data": "location" },
              { "data": "name" },
              { "data": "owner" },
              { "data": "address" },
              { "data": "city" },
              { "data": "permitStatus" },
              { "data": "activationDate" },
              { "data": "expiryDate" },
              {
                    "data": "encryptedId", "render": function (data, type, row, meta) {
                        if (row.isActive == true) {
                            if (Role == "SuperAdmin" || Role == "Admin" || Role == "Admin Inspector") {
                                return `<div class="m-75 btn-group"  role="group">
                                          <a class="btn btn-sm btn-custom"  href="/TFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
                                    </div>`
                            }
                            else if (Role == "Inspector") {
                                return `<div class="m-75 btn-group"  role="group">
                                          <a class="btn btn-sm btn-custom"  href="/TFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
                                    </div>`
                            }
                            else if (Role == "Clerk") {
                                return `<div class="m-75 btn-group"  role="group">
                                          <a class="btn btn-sm btn-custom"  href="/TFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
                                    </div>`
                            }
                            else {
                                return null
                            }
                        }
                        else {
                            return `<div class="m-75 btn-group"  role="group">      
                                              <a class="btn btn-sm btn-custom"  onclick=ActiveInactive('/ChangeUserState?id=${data}')><i class="fa-solid fa-check" id="icon"  style="cursor:pointer" title="Active"></i> </a>
                                        </div>`
                        }

                    }
              }
        ],
        columnDefs: [
            {
                targets: [1],
                render: function (data, type, row) {
                    if (data == null) {
                        return "--"
                    }
                    else {
                        var actDate = data.split("T");
                        actDate = moment(actDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                        return actDate;
                    }
                }
            },
            {
                targets: [2],
                render: function (data, type, row) {
                        if (data == null) {
                            return "--"
                        }
                        return data;
                }
            },
            {
                targets: [9],
                render: function (data, type, row) {
                    if (data == "Incomplete" || data == "Pending Admin Review" || data == "Admin Review" || data == "Pending Payment") {
                        return `<p class="badge bg-light">${data}</p>`
                    }
                    else if (data == "Active") {
                        return `<p class="badge bg-success">${data}</p>`
                    }
                    else if (data == "Inactive") {
                        return `<p class="badge bg-secondary">${data}</p>`
                    }
                    else if (data == "Expired") {
                        return `<p class="badge bg-danger">${data}</p>`
                    }
                    else {
                        return `<p class="badge bg-secondary">${data}</p>`
                    }
                }
            },
            
            {
                targets: [10],
                render: function (data, type, row) {
                    if (data == null) {
                        return "--"
                    }
                    else {
                        var actDate = data.split("T");
                        actDate = moment(actDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                        return actDate;
                    }
                }
            },
            {
                targets: [11],
                render: function (data, type, row) {
                    if (data == null) {
                        return "--"
                    }
                    else {
                        var expDate = data.split("T");
                        expDate = moment(expDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
                        return expDate;
                    }
                }
            },
        ],
        "language": {
            "emptyTable": "No records found"
        },
        "width": "100%",
        "createdRow": function (row, data, dataIndex) {
            if ($(data)[0].isActive == false) {
                $(row).css('color', 'red');
            }
        }
    });
}


//function loadTempDataTable(Role) {

//    dataTableTemp = $('#tempPermitIdx').DataTable({
//        "responsive": true,
//        "lengthChange": false,
//        "autoWidth": false,
//        "deferRender": true,
//        "ajax": {
//              "url": "/GetAllTempPermits"
//        },
//        "columns": [
//              {
//                    data: 'id',
//                    render: function (data, type, row, meta) {
//                          //console.log(row);
//                          return meta.row + /*meta.settings._iDisplayStart +*/ 1;
//                    }
//              },
//              { "data": "applicationNumber" },
//              { "data": "applicationDate" },
//              { "data": "permitNumber" },
//              { "data": "events" },
//              { "data": "location" },
//              { "data": "name" },
//              { "data": "permitStatus" },
//              { "data": "applicationFor" },
//              { "data": "activationDate" },
//              { "data": "expiryDate" },
//              {
//                    "data": "encryptedId", "render": function (data, type, row, meta) {
//                        if (row.isActive == true) {
//                            if (Role == "SuperAdmin" || Role == "Admin" || Role == "Admin Inspector") {
//                                return `<div class="m-75 btn-group"  role="group">
//                                          <a class="btn btn-sm btn-custom"  href="/TFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
//                                    </div>`
//                            }
//                            else if (Role == "Inspector") {
//                                return `<div class="m-75 btn-group"  role="group">
//                                          <a class="btn btn-sm btn-custom"  href="/TFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
//                                    </div>`
//                            }
//                            else if (Role == "Clerk") {
//                                return `<div class="m-75 btn-group"  role="group">
//                                          <a class="btn btn-sm btn-custom"  href="/TFEdit?id=${data}"><i class="fa fa-edit" aria-hidden="true" style="cursor:pointer" title="Edit"></i></a>
//                                    </div>`
//                            }
//                            else {
//                                return null
//                            }
//                        }
//                        else {
//                            return `<div class="m-75 btn-group"  role="group">      
//                                              <a class="btn btn-sm btn-custom"  onclick=ActiveInactive('/ChangeUserState?id=${data}')><i class="fa-solid fa-check" id="icon"  style="cursor:pointer" title="Active"></i> </a>
//                                        </div>`
//                        }

//                    }
//              }
//        ],
//        columnDefs: [
//              {
//                    targets: [3],
//                    render: function (data, type, row) {
//                          if (data == null) {
//                                return "--"
//                          }
//                          return data;
//                    }
//              },
//            //{
//            //    targets: [3],
//            //    render: function (data, type, row) {
//            //        if (data == null) {
//            //            return "NA"
//            //        }
//            //        return data;
//            //    }
//            //},
//            //{
//            //    targets: [4],
//            //    render: function (data, type, row) {
//            //        if (data == null) {
//            //            return "NA"
//            //        }
//            //        return data;
//            //    }
//            //},
//            {
//                targets: [7],
//                render: function (data, type, row) {
//                    if (data == "Incomplete" || data == "Pending Admin Review" || data == "Admin Review" || data == "Pending Plan Review" || data == "Plan Review" || data == "Pending Build-Out" || data == "Pending Payment" || data == "Opening Inspection") {
//                        return `<p class="badge bg-light">${data}</p>`
//                    }
//                    else if (data == "Active") {
//                        return `<p class="badge bg-success">${data}</p>`
//                    }
//                    else if (data == "Renewal") {
//                        return `<p class="badge bg-warning">${data}</p>`
//                    }
//                    else if (data == "Expired") {
//                        return `<p class="badge bg-danger">${data}</p>`
//                    }
//                    else if (data == "Expired") {
//                        return `<p class="badge bg-danger">${data}</p>`
//                    }
//                    else {
//                        return `<p class="badge bg-secondary">${data}</p>`
//                    }
//                    //else if (data == "Complaint") {
//                    //    return `<p class="badge bg-danger">Complaint</p>`
//                    //}
//                }
//            },
//            {
//                targets: [8],
//                render: function (data, type, row) {
//                    if (data == "NewPermit") {
//                        return `<p class="badge bg-success">New Permit</p>`
//                    }
//                    else if (data == "OwnerChange") {
//                        return `<p class="badge bg-warning">Owner Change</p>`
//                    }
//                    else if (data == "Complaint") {
//                        return `<p class="badge bg-danger">Complaint</p>`
//                    }
//                }
//            },
//            {
//                targets: [9],
//                render: function (data, type, row) {
//                    if (data == null) {
//                        return "--"
//                    }
//                    else {
//                        var actDate = data.split("T");
//                        actDate = moment(actDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
//                        return actDate;
//                    }
//                }
//            },
//            {
//                targets: [10],
//                render: function (data, type, row) {
//                    if (data == null) {
//                        return "--"
//                    }
//                    else {
//                        var expDate = data.split("T");
//                        expDate = moment(expDate[0], "YYYY-MM-DD").format("MM/DD/YYYY");
//                        return expDate;
//                    }
//                }
//            },
//        ],
//        "language": {
//            "emptyTable": "No records found"
//        },
//        "width": "100%",
//        "createdRow": function (row, data, dataIndex) {
//            if ($(data)[0].isActive == false) {
//                $(row).css('color', 'red');
//            }
//        }
//    });
//}


function ActiveInactivePermit(url, dataTableSelector, act, permitNumber) {
    var msg = 'Do you want to ' + act + ' the permit: ' + permitNumber
    $('audio#warning')[0].play();
    setTimeout(() => {
        Swal.fire({
            title: msg + `<br>` + 'Are you sure?',
            //text: 'Are you sure?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#8B9BB2',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "POST",
                    url: url,
                    success: function (data) {
                        if (data.success) {
                            $('audio#success_sound')[0].play();
                            setTimeout(() => {
                                toastr.success(data.msg);
                            }, 500)

                            //toastr.success(data.message);
                            $(dataTableSelector).DataTable().ajax.reload();
                        }
                    }
                });

            }
        })
    }, 100)

}

function GoToRenewPage(url, permitNumber) {
    $('audio#warning')[0].play();
    setTimeout(() => {
        Swal.fire({
            title: 'Are you sure you want to proceed to the Renewal Process of the Permit: ' + permitNumber + ' ?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#8B9BB2',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed!'
        }).then((result) => {
            if (result.isConfirmed) {
                location.href = url;
            }
        })
    }, 100)
}

function ClosePermit(url, dataTableSelector, permitNumber) {
    $('audio#warning')[0].play();
    setTimeout(() => {
        Swal.fire({
            title: 'You want to close the permit ' + permitNumber + '!! Are you sure? You cannot revert it!',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#8B9BB2',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, Proceed!'
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    type: "POST",
                    url: url,
                    success: function (data) {
                        if (data.success) {
                            $('audio#success_sound')[0].play();
                            setTimeout(() => {
                                toastr.success(data.msg);
                            }, 500)

                            //toastr.success(data.message);
                            $(dataTableSelector).DataTable().ajax.reload();
                        }
                    }
                });

            }
        })
    }, 100)

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
                $('#certificateMailEstId').val(data.id);
                
            }
        },
        complete: function () {
            $('#mailSend').modal('show');
        }

    })
}

function CertficateMailModalert() {
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

    
    var url = "/SendPermitCertificateViaEmail";
    
    var data = $('#SendMail').serialize();
    $.ajax({
        type: "POST",
        url: url,
        data: data,
        beforeSend: function () {
            $('div#loading-wrapper').hide();
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
            $('div#overlay').hide();
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

//function UpsertModal(url) {
//    $('#userUpsertForm').trigger('reset');
//    $.ajax({
//        type: "GET",
//        url: url,
//        beforeSend: function () {
//            $('div#loading-wrapper').show();
//        },
//        success: function (data) {
//            if (data.success) {
//                $('#userId').val(data.user.id);
//                $('#firstName').val(data.user.firstName);
//                $('#lastName').val(data.user.lastName);
//                $('#bhcd').val(data.user.bhcd);
//                $('#emailId').val(data.user.emailId);
//                $('#rSanitarian').val(data.user.registeredSanitarian);
//                $('#sanitarianTrain').val(data.user.sanitarianInTrain);
//                $('#designRepresentative').val(data.user.designatedRepresentative);
//                $('#poolOperator').val(data.user.certifiedPoolOperator);
//                $('#roleId').val(data.user.roleId);
//            }
//        },
//        error: function (data) {
//            console.log(data);
//        },
//        complete: function () {
//            $('div#loading-wrapper').hide();
//            $('#userUpsert').modal('show');
//        }
//    });
//}

