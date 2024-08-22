
showInPopup = (url, title) => {
    $.ajax({
        type: 'GET',
        url: url,
        success: function (res) {
            $('#form-modal .modal-body').html(res);
            $('#form-modal .modal-title').html(title);
            $('#form-modal').modal('show');

            // to make popup draggable
            $('.modal-dialog').draggable({
                handle: ".modal-header"
            });
        }
    })
}
   var Delete = function (button) {
       // $('table.display').on("click", ".btnDelete", function () {
       var btn = button;

        swal({
            title: 'Silme Onayı',
            text: 'Bu Kaydı Silmek İstiyormusunuz ?',
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: 'Kaydı Sil',
            closeOnConfirm: true,
            cancelButtonText: "Kapat"

        }, function (isConfirm) {
            if (isConfirm) { 
                $.ajax({
                    url: '/' + btn.attr("data-url"),
                    data: {
                        id: btn.data("id")
                    },
                    type: "post",
                    cache: false,
                    success: function (result) {
                        ShowMessage(result);
                        btn.parent().parent().remove();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {

                    }
                }); 
            }
        });    
}

function toCamelCase(iO) {
    iO.value = iO.value.replace(/([\wöçşğüıİ])/gi,
        function (a, b) { return b.replace("I", "ı").toLowerCase() }).replace(/(^[a-zöçşğüı]|[\s|\.][a-zöçşğüı])/g,
            function (c, d) { return d.replace("i", "İ").toUpperCase() });
}

var SaveControl = function (response) {
    if (response.Messages != null) {
        ShowMessage(response);
        if (response.ActionName != null  && response.TargetId != "") {
            $.ajax({
                url: '/' + response.ActionName,
                success: function (result) {
                    $('#' + response.TargetId).html(result);

                    if (response.TargetId != "divContent") {
                        $('#' + response.TargetId).DataTable().destroy();
                        $('#' + response.TargetId).DataTable({
                            dom: 'Bfrtip',
                            buttons: [
                                'pdf', 'excel'
                            ],
                            "pageLength": 25,
                            "bLengthChange": false,
                            "bLengthChange": false,
                            "ordering": false,
                            "language": {
                                "url": "//cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Turkish.json"
                            }
                        }).draw();
                    }
                }
            });
        }
        else if (response.ActionName != null && response.ActionName != "" && response.TargetId == ""){
            window.location.href = '/' + response.ActionName;
        }
    }   
}

var SaveControlAndRefresh = function (response) {
    if (response.Messages != null) {
        ShowMessage(response);
        RefreshGrid();
    }
}

var AddDays = function (strngDate, value) {
    
    strngDate = strngDate.split("/").join(".")
    var splitDate = strngDate.split('.');    
    var sBrowser, sUsrAg = navigator.userAgent;
    

    var date = null;
    if (sUsrAg.indexOf("Safari") > -1 && sUsrAg.indexOf("Chrome") == -1) {
        date = new Date(parseInt(splitDate[2]), parseInt(splitDate[1]) - 1, parseInt(splitDate[0]));
        date.setDate(date.getDate() + value);
        return date.getDate() + '.' + parseInt(date.getMonth() + 1) + '.' + date.getFullYear();
    }
    else {
        date = new Date(splitDate[2] + '-' + splitDate[1] + '-' + splitDate[0]);
        date.setDate(date.getDate() + value);
        return date.toLocaleDateString();
    }
}

var getImage = function (actionName, imgName) {

    var files = $("#fucImage").get(0).files;


    if (files.length > 0) {
        var fileData = new FormData();
        fileData.append(files[0].name, files[0]);

        if (imgName != '') {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#' + imgName)
                    .attr('src', e.target.result)
                    .width(150)
                    .height(200);
            };
        }

        reader.readAsDataURL(files[0]);

        if (actionName != '') {
            $.ajax({
                url: actionName,
                type: 'post',
                contentType: false, // Not to set any content header  
                processData: false, // Not to process data  
                data: fileData,
                success: function (response) {
                    ShowMessage(response);
                }
            });
        }
    }
}

var ShowMessage = function (response) {
    if (response == null)
        return;

    if (response.Messages == null)
        return;

    if (response.MessageType == 0) {
        var options = { 'closeButton': false, 'debug': false, 'newestOnTop': true, 'progressBar': false, 'positionClass': 'toast-top-right', 'preventDuplicates': false, 'onclick': null, 'showDuration': '300', 'hideDuration': '1000', 'timeOut': '3000', 'extendedTimeOut': '1000', 'showEasing': 'swing', 'hideEasing': 'linear', 'showMethod': 'fadeIn', 'hideMethod': 'fadeOut' }

        for (i = 0; i < response.Messages.length; i++) {
            if (response.MessageAction == 0) {
                toastr.success(response.Messages[i], '', options);
                if (response.CloseModal == true)
                    jQuery(document.getElementsByClassName('modal fade')).modal('hide');
            }
            else
                toastr.warning(response.Messages[i], '', options);
        }
    }

    if (response.MessageType == 1) {
        var status = "success";
        if (response.MessageAction == 1)
            status = "info";
        else if (response.MessageAction == 2)
            status = "warning";
        else if (response.MessageAction == 3)
            status = "error";
        if (response.CloseModal == true)
            jQuery(document.getElementsByClassName('modal fade')).modal('hide');
        swal({ title: response.Title, text: response.Messages[0], type: status, showCancelButton: false, confirmButtonText: 'Çıkış', closeOnConfirm: true }, function () { });
    }
};

function GetFileName(name, controlname) {
    document.getElementById(controlname).value = '';
    if (name.split('.').length == 2)
        document.getElementById(controlname).value = name.split('.')[0];
    else {
        for (i = 0; i < name.split('.').length - 1; i++) {
            if (document.getElementById(controlname).value.length == 0)
                document.getElementById(controlname).value += name.split('.')[i];
            else
                document.getElementById(controlname).value += '.' + name.split('.')[i];
        }
    }
}

function IsUploadAllowed(extensions, size, controlName) {

    if (document.getElementById(controlName).files.length == 0) {
        ShowMessage({ MessageType: "Notification", MessageAction: "Warning", Messages: ["Lütfen Yüklemek istediğiniz dosyayı seçiniz."] });
        document.getElementById(controlName).value = '';
        return false;
    }

    var filelist = document.getElementById(controlName).files;
    for (var i = 0; i < filelist.length; i++) {
        if (size < filelist[i].size) {
            ShowMessage({ MessageType: "Notification", MessageAction: "Warning", Messages: ["En fazla " + size / 1000 + " KB lık dosya yükleyebilirsiniz."] });
            document.getElementById(controlName).value = '';
            return false;
        }

        if (extensions.toUpperCase().indexOf(filelist[i].name.split('.').pop().toUpperCase()) < 0) {
            ShowMessage({ MessageType: "Notification", MessageAction: "Warning", Messages: ["Sadece " + extensions.toUpperCase() + " uzantılı dosya yükleyebilirsiniz."] });
            document.getElementById(controlName).value = '';
            return false;
        }
    }
    return true;
}

function GetFile(controlName, functionName, firstParameterName, IsDocumentRequired) {
    if (IsDocumentRequired == true && document.getElementById(controlName).value == '') {
        ShowMessage({ MessageType: "Notification", MessageAction: "Warning", Messages: ["Lütfen Yüklemek istediğiniz dosyayı seçiniz."] });
        return;
    }

    if (IsDocumentRequired == false && document.getElementById(controlName).value == '') {
        eval(functionName + '("' + firstParameterName + '")');
        return;
    }

    if (!window.FileReader) {
        alert('Browserınız bu işlemi desteklememektedir.');
        return;
    }

    var reader = new FileReader();
    reader.readAsDataURL(document.getElementById(controlName).files[0]);
    reader.onloadend = function () {
        eval(functionName + '("' + firstParameterName + ';' + document.getElementById(controlName).files[0].name + ';' + reader.result.split('base64,').pop() + '")');
    }
}

var names;
var body;
    function GetFiles(controlName, functionName, firstParameterName, IsDocumentRequired) {
        if (IsDocumentRequired == true && document.getElementById(controlName).value == '') {
        ShowMessage({ MessageType: "Notification", MessageAction: "Warning", Messages: ["Lütfen Yüklemek istediğiniz dosyayı seçiniz."] });
    return;
}

names = [];
body = [];
var filelist = document.getElementById(controlName).files;
        for (var i = 0; i < filelist.length; i++) {
        writefiles(filelist[i], controlName, functionName, firstParameterName);
    }
}


    function writefiles(file, controlName, functionName, firstParameterName) {
        var reader = new FileReader();
    reader.readAsDataURL(file);
        reader.onload = function () {
        names.push(file.name);
    body.push(reader.result.split('base64,').pop());
    if (document.getElementById(controlName).files.length == names.length)
        eval(functionName + '.PerformCallback("' + firstParameterName + ';' + names + ';' + body + '");');
}
}

    function ShowDocumentAndOpenNewWindow(parameter) {
        var newWindow;
    var top = (screen.availHeight - 600) / 2;
    var left = (screen.availWidth - 1000) / 2;

    if (parameter.indexOf('WhatsAppConnectorPage') > -1)
        newWindow = window.open(parameter, 'NewWindowPage', 'width=800,height=600,resizable=1,scrollbars=1,status=no,top=top,left=left');
    else
        newWindow = window.open(parameter, 'NewWindowPage', 'width=1000,height=650,resizable=1,scrollbars=1,status=no,top=top,left=left');
    newWindow.focus();
}

    function ShowDocumentAndOpenNewTab(parameter) {
        window.open(parameter, '_newtab');
    }

    function ShowDocumentAndOpenAddTab(parameter) {
        window.open(parameter, '_blank');
}