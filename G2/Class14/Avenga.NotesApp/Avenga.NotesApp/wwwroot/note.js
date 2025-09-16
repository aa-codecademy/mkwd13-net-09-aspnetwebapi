let getAllNotesBtn = document.getElementById("btn1");
let addNoteBtn = document.getElementById("btn3");
let logoutBtn = document.getElementById("logoutBtn");
let addNoteTextInput = document.getElementById("noteText");
let addNotePriorityInput = document.getElementById("notePriority");
let addNoteTaginput = document.getElementById("noteTag");
let addNoteUserInput = document.getElementById("noteUserId");

let url = "http://localhost:5280/api/notes";

let getAllNotes = async () => {
    //getting the token from localStorage
    let token = localStorage.getItem("notesApiToken");

    let response = await fetch(url, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`
        }
    })

    if (response.ok) {
        let notes = await response.json();
        console.log(notes);
        displayNotes(notes)
    } else {
        console.log(`failed to fetch notes:`, response.statusText)
    }
}

let addNote = async () => {

    //getting the token from localStorage
    let token = localStorage.getItem("notesApiToken");

    let numberUserId = parseInt(addNoteUserInput.value);
    let priorityInt = parseInt(addNotePriorityInput.value);
    let TagInt = parseInt(addNoteTaginput.value);

    console.log(numberUserId);

    let note = {
        Text: addNoteTextInput.value,
        Priority: priorityInt,
        Tag: TagInt,
        UserId: numberUserId
    }

    let response = await fetch(url + "/addNote", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(note)
    })
        .then(function (response) {
            console.log(response);
        })
        .catch(function (err) {
            console.log(err);
        })
}

let displayNotes = (notes) => {
    //creating the elements
    let table = document.createElement("table");
    let thead = document.createElement("thead");
    let tbody = document.createElement("tbody");

    //table headers
    let headers = ["Text", "Priority", "Tag", "UserId"]
    let headerRow = document.createElement("tr");
    headers.forEach(headerText => {
        let th = document.createElement("th");
        th.textContent = headerText;
        headerRow.appendChild(th);
    });
    thead.appendChild(headerRow);

    //table rows
    notes.forEach(note => {
        let row = document.createElement("tr");
        Object.values(note).forEach(value => {
            let td = document.createElement("td");
            td.textContent = value;
            row.appendChild(td);
        });
        tbody.appendChild(row);
    })
    table.appendChild(thead);
    table.appendChild(tbody);

    let tableContainer = document.getElementById("tableContainer");
    tableContainer.innerHTML = "";
    tableContainer.appendChild(table);
}


let logout = () => {
    localStorage.removeItem("notesApiToken") // this should remove the token from localstorage
    window.location.href = "http://localhost:5280/login.html"
}

logoutBtn.addEventListener("click", logout);
getAllNotesBtn.addEventListener("click", getAllNotes);
addNoteBtn.addEventListener("click", addNote);