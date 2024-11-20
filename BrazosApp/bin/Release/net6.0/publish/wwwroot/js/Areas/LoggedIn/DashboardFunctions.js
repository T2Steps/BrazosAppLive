function getUsers() {
      $.ajax({
            type: "GET",
            url: '/GetAllRegisteredUsers',
            success: function (data) {
                  console.log(data);
                  if (data.success) {
                        //alert(data.count);
                        $('#loadingb1').hide();
                        $('#users').show();
                        $('#users').text(data.count);
                  }
                  else {
                        console.log("error");
                  }
                  //$('div#loading-wrapper').hide();
            },
            error: function (data) {
                  console.log(data);
            }
      });
}