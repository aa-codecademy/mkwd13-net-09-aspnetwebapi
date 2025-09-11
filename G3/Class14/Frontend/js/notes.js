const NOTES_API = "http://localhost:5062/api/Notes";

const priorityMap = 
{
    "Low": 1,
    "Medium": 2,
    "High": 3
};
const tagMap = 
{
    "Work": 1,
    "Health": 2,
    "SocialLife": 3
};

//Load notes
async function loadNotes() {
    try{
        const res = await fetch(NOTES_API, {
            method: "GET",
            headers: {"Authorization": "Bearer " + token}
        });
        if(!res.ok) throw new Error(await res.text());
        const notes = await res.json();
        renderNotes(notes);
    }
    catch(e){
        alert("Load notes failed:" + e.message);
    }
}
// Render notes
function renderNotes(notes){
    const container = document.getElementById("notesCards");
    container.innerHTML = "";
    notes.forEach(note => {
        const div = document.createElement("div");
        div.className = "col";
        div.innerHTML = `
            <div class="note-card card p-2">
                <div class="card-actions mb-2">
                    <button class="btn btn-sm btn-warning me-1"
                        onclick="showUpdateModal(${note.id},'${note.text}','${note.priority}','${note.tag}')">Edit</button>
                    <button class="btn btn-sm btn-danger" onclick="deleteNote(${note.id})">Delete</button>
                </div>
                <h5>${note.text}</h5>
                <p>Priority: ${priorityReverseMap[note.priority] || "Low"} | Tag: ${tagReverseMap[note.tag] || ""}</p>
            </div>
        `;
        container.appendChild(div);
    });
}

//add new note
document.getElementById("addNoteBtn").addEventListener("click", async()=>
    {
        const text = document.getElementById("newNoteText").value;
        const priority = priorityMap[document.getElementById("newNotePriority").value];
        const tag = tagMap[document.getElementById("newNoteTag").value];

        if(!text) return alert ("Enter note text");
        if(!priority) return alert ("Enter note priority");
        if(!tag) return alert ("Enter note tag");

        try{
            const response = await fetch(NOTES_API, 
                {
                    method: "POST",
                    headers: 
                    {
                        "Content-Type": "application/json",
                        "Authorization": "Bearer " + token
                    },
                    body: JSON.stringify({text, priority, tag})
                });
                if(!response.ok) throw new Error(await response.text());
                document.getElementById("newNoteText").value = "";
                loadNotes();
        }
        catch(e){
            alert("Add note failed: " + e.message);
        }
    }
);

// Delete note
async function deleteNote(id){
    if(!confirm("Delete this note?")) return;
    try{
        const res = await fetch(`${NOTES_API}/${id}`, {
            method:"DELETE",
            headers:{ "Authorization":"Bearer " + token }
        });
        if(!res.ok) throw new Error(await res.text());
        loadNotes();
    }catch(e){ alert("Delete failed: "+e.message);}
}

const priorityReverseMap = 
{ 
    1: "Low", 
    2: "Medium", 
    3: "High" 
};
const tagReverseMap = 
{ 
    1: "Work", 
    2: "Health", 
    3: "SocialLife" 
};
let currentUpdateNoteId = null;

//Update note
function showUpdateModal(id, text, priorityInt, tagInt){
    currentUpdateNoteId = id; 
    const modal = new bootstrap.Modal(document.getElementById("updateNoteModal"));
    document.getElementById("updateNoteText").value = text;
    document.getElementById("updateNotePriority").value = priorityReverseMap[priorityInt];
    document.getElementById("updateNoteTag").value = tagReverseMap[tagInt];
    modal.show();
}
document.getElementById("updateNoteForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const id = currentUpdateNoteId; 
    const text = document.getElementById("updateNoteText").value.trim();
    const priority = priorityMap[document.getElementById("updateNotePriority").value];
    const tag = tagMap[document.getElementById("updateNoteTag").value];

    if(!id) return alert("Note ID is missing!");
    if(!text) return alert("Text is required!");

    try {
        const res = await fetch(`${NOTES_API}`, {
            method: "PUT",
            headers: { 
                "Content-Type": "application/json",
                "Authorization": "Bearer " + token
            },
            body: JSON.stringify(
                 { id, text, priority, tag }
            )
        });

        if(!res.ok){
            const errorText = await res.text();
            throw new Error(errorText);
        }

        bootstrap.Modal.getInstance(document.getElementById("updateNoteModal")).hide();
        loadNotes();
    } catch(e){
        alert("Update failed: " + e.message);
    }
});