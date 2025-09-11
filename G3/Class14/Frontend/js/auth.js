const API_BASE = "http://localhost:5062/api";
const USERS_API = `${API_BASE}/Users`;
let token = localStorage.getItem("jwt") || "";

//Show forms
function showLogin(){
    document.getElementById("authChoice").style.display = "none";
    document.getElementById("loginForm").style.display = "block";
    document.getElementById("registerForm").style.display = "none";
}
function showRegister(){
    document.getElementById("authChoice").style.display = "none";
    document.getElementById("loginForm").style.display = "none";
    document.getElementById("registerForm").style.display = "block";
}
function showNotesContainer(){
    document.getElementById("authChoice").style.display = "none";
    document.getElementById("loginForm").style.display = "none";
    document.getElementById("registerForm").style.display = "none";
    document.getElementById("notesContainer").style.display = "block";
    loadNotes(); // from notes.js
}

//Login

document.getElementById("loginBtn").addEventListener("click", async()=>{
    const username = document.getElementById("loginUsername").value;
    const password = document.getElementById("loginPassword").value;

    try{
        const response = await fetch(`${USERS_API}/login`,
            {
                method: "POST",
                headers: {"Content-Type":"application/json"},
                body: JSON.stringify({username, password})
            });
            if(!response.ok) throw Error(await response.text());
            token = await response.text();
            localStorage.setItem("jwt", token);
            showNotesContainer();
    }
    catch(e)
    {
        alert("Login failed: " + e.message);
    }
})

// Register
document.getElementById("registerBtn").addEventListener("click", async () => {
    const firstName = document.getElementById("regFirstName").value;
    const lastName = document.getElementById("regLastName").value;
    const username = document.getElementById("regUsername").value;
    const password = document.getElementById("regPassword").value;
    const confirmPassword = document.getElementById("regConfirmPassword").value;

    try{
        const res = await fetch(`${USERS_API}/register`, {
            method:"POST",
            headers:{ "Content-Type":"application/json" },
            body: JSON.stringify({ firstName, lastName, username, password, confirmedPassword: confirmPassword })
        });
        if(!res.ok) throw new Error(await res.text());
        alert("Register success! Please login.");
        showLogin();
    }catch(e){ alert("Register failed: "+e.message);}
});

// Logout
document.getElementById("logoutBtn").addEventListener("click", () => {
    localStorage.removeItem("jwt");
    token = "";
    document.getElementById("notesContainer").style.display = "none";
    document.getElementById("authChoice").style.display = "block";
});