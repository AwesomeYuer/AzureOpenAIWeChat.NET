// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.
const textarea = document.getElementById("Topic");
textarea.addEventListener('input', () => {
    document.querySelectorAll('button').forEach(button => {
        button.disabled = textarea.value == '';
    });
});

document.querySelectorAll('button').forEach(button => {
    button.disabled = textarea.value == '';
});

document.getElementById('Topic').oninput = function () {
    let value = document.getElementById('Topic').value;
    let replaced = value.replace(/[\u4e00-\u9fa5]/g, 'aa');
    document.getElementById('current').innerText = replaced.length;
    if (replaced.length > 100) {
        //document.getElementById("ppt-btn").disabled = true;
        //document.getElementById("more-btn").disabled = true;
    }
    else {
        //document.getElementById("ppt-btn").disabled = false;
        //document.getElementById("more-btn").disabled = false;
    }

    if (replaced.length + 2 > 2000) {
        document.getElementById('Topic').value = value.substring(0, value.length - 1);
    }
}

//document.getElementById('ppt-btn').addEventListener('click', function (e) {
//    e.preventDefault();

//    var btnName = 'ppt-btn';
//    var mbtnName = 'more-btn';
//    document.getElementById("format").value = "PPT";
//    document.querySelectorAll('button').forEach(button => {
//        button.disabled = true;
//    });

//    rotateText = ['PowerPoint.', 'PowerPoint..', 'PowerPoint...'];

//    var i = 0;
//    var refreshInterval = setInterval(function () {
//        document.getElementById(btnName).innerHTML = rotateText[i];
//        i = (i + 1) % rotateText.length;
//    }, 500);

//    var formData = new FormData(document.getElementById('form'));
//    var xhr = new XMLHttpRequest();
//    xhr.open('POST', 'Index', true);
//    xhr.responseType = 'arraybuffer';
//    xhr.send(formData);

//    xhr.onload = function () {
//        if (xhr.status == 200) {
//            let fileResponse = xhr.response;
//            var filename = getDownloadFileName(xhr.getResponseHeader("Content-Disposition"));
//            var blob = new Blob([fileResponse], { type: 'application/vnd.ms-powerpoint' });
//            var link = document.createElement('a');
//            link.href = window.URL.createObjectURL(blob);
//            link.download = filename;
//            document.body.appendChild(link);
//            link.click();
//            document.body.removeChild(link);

//            // enable submit button
//            document.getElementById(btnName).innerHTML = "PowerPoint";
//            document.querySelectorAll('button').forEach(button => {
//                button.disabled = false;
//            });
//            clearInterval(refreshInterval);
//        }
//    }
//});


getDownloadFileName = function (disposition) {
    var utf8FilenameRegex = /filename\*=UTF-8''([\w%\-\.]+)(?:; ?|$)/i;
    var asciiFilenameRegex = /^filename=(["']?)(.*?[^\\])\1(?:; ?|$)/i;

    var fileName;
    if (utf8FilenameRegex.test(disposition)) {
        fileName = decodeURIComponent(utf8FilenameRegex.exec(disposition)[1]);
    } else {
        var filenameStart = disposition.toLowerCase().indexOf('filename=');
        if (filenameStart >= 0) {
            var partialDisposition = disposition.slice(filenameStart);
            var matches = asciiFilenameRegex.exec(partialDisposition);
            if (matches != null && matches[2]) {
                fileName = matches[2];
            }
        }
    }
    return fileName;
}
function submitForm(id) {

    document.querySelectorAll('button').forEach(button => {
        button.disabled = true;
    });

    var rotateText = ['Submit.', 'Submit..', 'Submit...'];
    var btnName = 'submit-btn';

    var i = 0;
    var timer = setInterval(function () {
        document.getElementById(btnName).innerHTML = rotateText[i];
        i = (i + 1) % rotateText.length;
    }, 500);

    var topic = $("#Topic").val();

    $.ajax({
        //url: "https://eu-aoai-ins-001.openai.azure.com/api/OpenAI", 
        url: "https://localhost:7103/api/OpenAI",
        type: 'POST',
        contentType: "application/json;charset=utf-8",
        async: true,
        dataType: 'json',
        timeout: 20000,
        data: JSON.stringify({
            User: topic, Bot: '', History: ''
        }),
        success: function (resultData) {
            console.log(resultData);
            $("#result").show();
            if (resultData.bot)
                $("#result").html(resultData.bot);
            else
                $("#result").html('让我想想...');

        },
        error: function (e) {
            console.log("系统出错！");
            console.log(e.error);
        },
        complete: function (xhr, status) {
            clearInterval(timer);
        },
    });
}

$("#fav-form3").submit(function (e) {

    e.preventDefault();

    var form = $(this);
    var actionUrl = form.attr('action');

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: form.serialize(),
        success: function (data) {
            console.log(data);
        }
    });
    $(this).find('.favbtn3').addClass('sel').attr('type', 'button');
    return false;
});
$("#fav-form35").submit(function (e) {

    e.preventDefault();

    var form = $(this);
    var actionUrl = form.attr('action');

    $.ajax({
        type: "POST",
        url: actionUrl,
        data: form.serialize(),
        success: function (data) {
            console.log(data);
        }
    });
    $(this).find('.favbtn35').addClass('sel').attr('type', 'button');
    return false;
});