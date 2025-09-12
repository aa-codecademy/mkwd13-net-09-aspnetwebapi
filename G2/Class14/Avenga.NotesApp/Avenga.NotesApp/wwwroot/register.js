let regBtn = document.getElementById("regBtn");
let fn = document.getElementById("firstName");
let ln = document.getElementById("lastName");
let username1 = document.getElementById("username");
let pass = document.getElementById("pass");
let confPass = document.getElementById("confPass");

let port = "5280";

let register = async () => {
    let url = "http://localhost:" + port + "/api/users/register";
    let user = {
        username: username1.value,
        FirstName: fn.value,
        LastName: ln.value,
        Password: pass.value,
        ConfirmPassword: confPass.value
    };

    let response = await fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(user)
    }).then(function (response) {
        console.log(response);
        window.location.href = "http://localhost:" + port + "/login.html"
    }).catch(function (error) {
        console.log(error)
    });
}

regBtn.addEventListener("click", register);