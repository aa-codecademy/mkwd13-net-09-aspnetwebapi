let usernameInput = document.getElementById("username");
let passInput = document.getElementById("pass");
let loginBtn = document.getElementById("loginBtn");
let port = "5280";

let login = async () => {
    
    let url = "http://localhost:" + port + "/api/users/login";

    //first we need to get the values from the input
    let user = {
        Username: usernameInput.value,
        Password: passInput.value
    }

    console.log(user);


    //afterwards we create the post with FETCH for the appropriate URL
    //then we store the token that the endpoint created in the browsers local storage
    let response = await fetch(url, {
        //we set bellow what kind of http method this function will trigger (GET/POST/PUT/DELETE)
        method: 'POST',
        //we set the headers and put only what type of content will be
        headers: {
            'Content-type': 'application/json'
        },
        //here we set the value in the body that will be send
        //and for that purpose we stringify the model
        //or we convert the values into JSON string
        body: JSON.stringify(user)
    })
    //here we make the response that we get from the endpoint
        .then(function (response) {
            console.log(response);
            response.text()
                .then(function (text) {
                    //here we save the token into our local storage in the browser
                    console.log(text);
                    localStorage.setItem("notesApiToken", text);
                    debugger;
                    //after everything is finished, we are redireted to the notes view
                    window.location.href = "http://localhost:5280/note.html"
                })
        })
        //here we are catching the error if it happens and logging it into the console
        .catch(function (err) {
            console.log(err)
        })
}

//we are adding the event listener to the button that we created so when we press with click
//the function login will be executed
loginBtn.addEventListener("click", login);