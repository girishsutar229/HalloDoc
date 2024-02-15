
//custome js

//Password icone show the passwors or not......
if (document.getElementById("togglePassword")) {

    const togglePassword = document.querySelector("#togglePassword");
    const password = document.querySelector("#password");
    togglePassword.addEventListener("click", function () {
        const type = password.getAttribute("type") === "password" ? "text" : "password";
        password.setAttribute("type", type);
        this.classList.toggle("bi-eye");
    });

}


// file name for uploading the documents
function GetFileSizeNameAndType() {
    var fi = document.getElementById('patientFile'); 
    var totalFileSize = 0;
    if (fi.files.length > 0) {
        document.getElementById('fileName').innerHTML += ' :- Total is' + '<b> ' + fi.files.length + ' </b>';
    }

}

// pop up box show in family submit butoon information submit......
document.addEventListener('DOMContentLoaded', function () {
    var myModal = new bootstrap.Modal(document.getElementById('customModal'));
    myModal.show();
});  

//Phonenumbershow with flages for  phone1......................................................................
var input = document.querySelector("#phone");
var iti = window.intlTelInput(input, {
    initialCountry: "in",
    separateDialCode: true,
    utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js"
});

var input = document.querySelector("#phone2");
var iti = window.intlTelInput(input, {
    initialCountry: "in",
    separateDialCode: true,
    utilsScript: "https://cdnjs.cloudflare.com/ajax/libs/intl-tel-input/17.0.8/js/utils.js"
});

// Update the flag when the country is changed
input.addEventListener("countrychange", function () {
    var countryCode = iti.getSelectedCountryData().iso2;
    var flagContainer = document.querySelector("#flag-container");
    flagContainer.innerHTML = "";
    var flag = document.createElement("div");
    flag.classList.add("iti__flag");
    flag.classList.add("iti__" + countryCode.toLowerCase());
    flagContainer.appendChild(flag);
});

// Validate phone number
function validatePhoneNumber() {
    if (iti.isValidNumber()) {
        document.getElementById("textChange").innerHTML = "Valid";
        document.getElementById("textChange").classList.remove("invalid-text");
        document.getElementById("textChange").classList.add("valid-text");
    } else {
        document.getElementById("textChange").innerHTML = "Invalid";
        document.getElementById("textChange").classList.remove("valid-text");
        document.getElementById("textChange").classList.add("invalid-text");
    }
}
