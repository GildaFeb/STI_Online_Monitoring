
function myFunction() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("myInput");
    filter = input.value.toUpperCase();
    table = document.getElementById("myTable");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        td = tr[i].getElementsByTagName("td")[0];
        if (td) {
            txtValue = td.textContent || td.innerText;
            if (txtValue.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}
//Add Data Function   
function AddRequest() {
    var res = validateGuest();
    if (res == false) {
        return false;
    }
    var empObj = {
        GuestID: $('#jGuestID').val(),
        DateOfVisit: $('#jDateOfVisit').val(),
        Department: $('#jDepartment').val(),
        Transactions: $('#jTransaction').val()
    };
    var ans = confirm("Add Request Form. Do you want to Continue?");
    if (ans) {
        $.ajax({
            url: "/LogIn/Add",
            data: JSON.stringify(empObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",
            success: function (result) {

                $('#RequestModal').modal('hide');
                alert("Form Request Added Successfully.");
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        })
   
    }
}

//function for updating employee's record  
function UpdateRequest() {
    var res = validateGuest();
    if (res == false) {
        return false;
    }
    var empObj = {
        DateOfVisit: $('#DateOfVisit').val(),
        TimeIn: $('#jTimeIn').val(),
        TimeOut: $('#jTimeOut').val(),
        Department: $('#jDepartment').val(),
        Transactions: $('#jTransaction').val()
    };
    $.ajax({
        url: "/LogIn/Update",
        data: JSON.stringify(empObj),
        type: "POST",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            loadData();
            $('#jRequestModal').modal('hide');
            $('#jGuestID').val("");
            $('#jDateOfVisit').val("");
            $('#jTimeIn').val("");
            $('#jTimeOut').val("");
            $('#jDepartment').val("");
            $('#jTransaction').val("");
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

//function for deleting employee's record  
function Delele(ID) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/LogIn/Delete/" + ID,
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                loadData();
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

//Function for clearing the textboxes  
function clearTextBox() {
    var res = validateGuest();
    if (res == false) {
        return false;
    }  
    $('#jDateOfVisit').val("");
    $('#jDepartment').val("");
    $('#jTransaction').val("");
    $('#jbtnUpdate').hide();
    $('#jbtnAdd').show();
    $('#jDateOfVisit').css('border-color', 'lightgrey');
    $('#jTimeIn').css('border-color', 'lightgrey');
    $('#jTimeOut').css('border-color', 'lightgrey');
    $('#jDepartment').css('border-color', 'lightgrey');
    $('#jTransaction').css('border-color', 'lightgrey');
}
//Valdidation using jquery  
function validateGuest() {
    var isValid = true;
    if ($('#jGuestID').val().trim() == "") {
        $('#jGuestID').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#jGuestID').css('border-color', 'lightgrey');
    }
    if ($('#jDateOfVisit').val().trim() == "") {
        $('#jDateOfVisit').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#jDateOfVisit').css('border-color', 'lightgrey');
    }
   
    if ($('#jDepartment').val().trim() == "") {
        $('#jDepartment').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#jDepartment').css('border-color', 'lightgrey');
    }
    if ($('#jTransaction').val().trim() == "") {
        $('#jTransaction').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#jTransaction').css('border-color', 'lightgrey');
    }
    return isValid;
}
//===========================================================================================
//=================================== GUEST INFORMATION =====================================
// ==========================================================================================




//Load Data function  
function loadData() {
    $.ajax({
        url: "/LogIn/List",
        type: "GET",
        contentType: "application/json;charset=utf-8",
        dataType: "json",
        success: function (result) {
            var html = '';
            $.each(result, function (key, item) {
                html += '<tr>';
                html += '<td>' + item.GuestID + '</td>';
                html += '<td>' + item.ContactNumber + '</td>';
                html += '<td>' + item.Address + '</td>';
                html += '<td>' + item.Password + '</td>';
                html += '<td><a href="#" onclick="return getbyID(' + item.GuestID + ')">Edit</a> | <a href="#" onclick="Delele(' + item.EmployeeID + ')">Delete</a></td>';
                html += '</tr>';
            });
            $('.tbody').html(html);
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function EditInfo(idGuest) {

    $.ajax({
        url: "/LogIn/getbyID/" + idGuest,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#GuestID').val(result.GuestID);
            $('#ContactNumber').val(result.ContactNumber);
            $('#Address').val(result.Address);
            $('#password').val(result.Password);
            $('#LastName').val(result.LasttName);
            $('#FirstName').val(result.FirstName);
            $('#MiddleName').val(result.MiddleName);
            $('#Suffix').val(result.Suffix);
            $('#Gender').val(result.Gender);
            $('#Email').val(result.Email);
            $('#EditInformation').modal('show');
            $('#btnUpdate').show();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
}



function Update() {
    var res = validate();
    if (res == false) {
        return false;
    }
    var GuestObj = {
        GuestID: $('#GuestID').val(),
        ContactNumber: $('#ContactNumber').val(),
        Age: $('#Age').val(),
        Address: $('#Address').val(),
        Password: $('#password').val(),
        LasttName: $('#LastName').val(),
        FirstName: $('#FirstName').val(),
        MiddleName: $('#MiddleName').val(),
        Suffix: $('#Suffix').val(),
        Gender: $('#Gender').val(),
        Email: $('#Email').val()
    };
        var ans = confirm("Update Guest Information. Do you want to Continue?");
        if (ans) {
        $.ajax({
            url: "/LogIn/Update",
            data: JSON.stringify(GuestObj),
            type: "POST",
            contentType: "application/json;charset=utf-8",
            dataType: "json",

            success: function (result) {
                loadData();
                $('#EditInformation').modal('hide');
                $('#Information').modal('hide');
                $('#GuestID').val("");
                $('#ContactNumber').val("");
                $('#Address').val("");
                $('#password').val("");
                $('#LastName').val("");
                $('#FirstName').val("");
                $('#MiddleName').val("");
                $('#Suffix').val("");
                $('#Gender').val("")
                $('#Email').val("")

                alert("Information Successfully Updated");
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        }
   
    });
}

$(document).ready(function () {
    $("#cpassword").keyup(validate);
    $("#password").key(validate);
    $("#ContactNumber").key(validate);
    $("#Address").key(validate);
    $("#cpassword").key(validate);
});


function validate() {
    var password1 = $("#password").val();
    var password2 = $("#cpassword").val();

    if (password1 == password2) {
        $("#alert").text("Password matched");
        $('#alert').css('color', 'green');
    }
    else {
        $("#alert").text("Not matched");
    }
    var isValid = true;
    if ($('#password').val().trim() == "") {
        $('#password').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#password').css('border-color', 'lightgrey');
    }
    if ($('#Address').val().trim() == "") {
        $('#Address').css('border-color', 'Red');
        isValid = false;
    }
    else {
        $('#Address').css('border-color', 'lightgrey');
    }
    if ($('#ContactNumber').val().trim() == "") {
        $('#ContactNumber').css('border-color', '');
        isValid = false;
    }
    else {
        $('#ContactNumber').css('border-color', 'lightgrey');
    }
    if ($('#cpassword').val().trim() == "") {
        $('#cpassword').css('border-color', '');
        isValid = false;
    }
    else {
        $('#cpassword').css('border-color', 'lightgrey');
    }
}

//===========================================
//============ GET CONFIRMATION =============
//===========================================

function getConfirmation(id) {

    $.ajax({
        url: "/LogIn/GetbyID/" + id,
        typr: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $('#idGuest').val(GuestID);


            $('#MyModal').modal('show');
            $('#btnConfirm').show();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
    return false;
} 