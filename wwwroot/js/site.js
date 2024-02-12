// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


//custome js

if (document.getElementById("togglePassword")) {

    const togglePassword = document.querySelector("#togglePassword");
    const password = document.querySelector("#password");

    togglePassword.addEventListener("click", function () {
        // toggle the type attribute
        const type = password.getAttribute("type") === "password" ? "text" : "password";
        password.setAttribute("type", type);

        // toggle the icon
        this.classList.toggle("bi-eye");
    });

}

if (document.getElementById("phone")) {
    //submit types js  for phone country code input.................................................................................................................................
    const phoneInputField = document.querySelector("#phone");
    const phoneInput = window.intlTelInput(phoneInputField, {
        utilsScript:
            "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
    });
}

if (document.getElementById("phone2")) {
    const phoneInputField2 = document.querySelector("#phone2");
    const phoneInput2 = window.intlTelInput(phoneInputField2, {
        utilsScript:
            "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js",
    });
}

// file name
function GetFileSizeNameAndType() {
    var fi = document.getElementById('patientFile'); // GET THE FILE INPUT AS VARIABLE.
    var totalFileSize = 0;

    // VALIDATE OR CHECK IF ANY FILE IS SELECTED.
    if (fi.files.length > 0) {
        // RUN A LOOP TO CHECK EACH SELECTED FILE.
        for (var i = 0; i <= fi.files.length - 1; i++) {
            //ACCESS THE SIZE PROPERTY OF THE ITEM OBJECT IN FILES COLLECTION. IN THIS WAY ALSO GET OTHER PROPERTIES LIKE FILENAME AND FILETYPE
            var fsize = fi.files.item(i).size;
            totalFileSize = totalFileSize + fsize;
            document.getElementById('fileName').innerHTML = ' <b>' + fi.files.item(i).name + ' <b>';
        }
    }
}

// pop up box show in family submit butoon information submit...................................................................................................................................
// Add this script at the end to initialize Bootstrap JavaScript

document.addEventListener('DOMContentLoaded', function () {
    var myModal = new bootstrap.Modal(document.getElementById('customModal'));
    myModal.show();
});  
