var fields = document.getElementById("profileForm").getElementsByTagName('*');
var editbtn = document.getElementById("editbtn");
var cancelbtn = document.getElementById("cancelbtn");
var submitbtn = document.getElementById("submitbtn");
editbtn.addEventListener("click", () => {
    editbtn.classList.add("d-none");
    cancelbtn.classList.remove("d-none");
    submitbtn.classList.remove("d-none");
    for (var i = 0; i < fields.length; i++) {
        fields[i].disabled = false;
    }
    document.getElementById("Email").disabled = true;
});
//Location Scripted

$(document).ready(function () {
    $("#openBtn").click(function () {
        $("#myModal1").modal("show");
        var Street = $("#street").val();
        var City = $("#city").val();
        var State = $("#state").val();
        var ZipCode = $("#zipcode").val();
        var address = "https://maps.google.com/maps?q=" + Street + City + State + ZipCode + "&t=&z=13&ie=UTF8&iwloc=&output=embed";
        $("#gmap_canvas").attr("src", address);
    });
});

$(document).ready(function () {
    $("#closeBtn1").click(function () {
        $("#myModal1").modal("hide");
    });
});