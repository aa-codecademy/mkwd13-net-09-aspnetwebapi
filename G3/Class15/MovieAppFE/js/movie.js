const apiUrl = 'http://localhost:5236/api/Movies';
const token = JSON.parse(localStorage.getItem('user'));

    if (!token) {
        alert("Unauthorized! Redirecting to login...");
        window.location.href = "index.html";
    }

    const headers = {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
    };
function renderMovieList(movies) {
    const list = document.getElementById("movieList");
    list.innerHTML = "";

    if (movies.length === 0) {
        list.innerHTML = `<p class="text-center">No movies found.</p>`;
        return;
    }

    movies.forEach(movie => {
        list.innerHTML += `
            <div class="col-md-4">
                <div class="card mb-3">
                    <div class="card-body">
                        <h5 class="card-title">${movie.title}</h5>
                        <p class="card-text">${movie.description}</p>
                        <p class="card-text"><small class="text-muted">Year: ${movie.year}</small></p>
                        <p class="card-text"><small class="text-muted">Genre: ${getGenreName(movie.genre)}</small></p>
                        <button class="btn btn-sm btn-primary me-2" onclick='openEditModal(${JSON.stringify(movie)})'>Edit</button>
                        <button class="btn btn-sm btn-danger" onclick='deleteMovie(${movie.id})'>Delete</button>
                    </div>
                </div>
            </div>
        `;
    });
}

    // Load all movies
    async function loadMovies() {
        const res = await fetch(apiUrl, { headers });
        const movies = await res.json();
        const list = document.getElementById("movieList");
        list.innerHTML = "";

        renderMovieList(movies);

    }
    // Filter movies
document.getElementById("filterForm").addEventListener("submit", async function (e) {
    e.preventDefault();

    const year = document.getElementById("filterYear").value;
    const genre = document.getElementById("filterGenre").value;

    let url = `${apiUrl}/filterMovie?`;

    if (year) url += `year=${year}&`;
    if (genre !== "") url += `genre=${genre}`;

    const res = await fetch(url, { headers });

    if (!res.ok) {
        alert("Error filtering movies.");
        return;
    }

    const movies = await res.json();
    renderMovieList(movies);
});


    // Add movie
    document.getElementById("addMovieForm").addEventListener("submit", async function (e) {
        e.preventDefault();
        const movie = {
            title: document.getElementById("title").value,
            description: document.getElementById("description").value,
            year: parseInt(document.getElementById("year").value),
            genre: parseInt(document.getElementById("genre").value)
        };

        const res = await fetch(apiUrl, {
            method: "POST",
            headers,
            body: JSON.stringify(movie)
        });

        if (res.status === 201) {
            loadMovies();
            this.reset();
        } else {
            alert("Error adding movie.");
        }
    });

    // Delete movie
    async function deleteMovie(id) {
        if (!confirm("Are you sure?")) return;

        const res = await fetch(`${apiUrl}/${id}`, {
            method: "DELETE",
            headers
        });

        if (res.status === 204) {
            loadMovies();
        } else {
            alert("Error deleting movie.");
        }
    }

    // Open modal for editing
    function openEditModal(movie) {
        document.getElementById("editId").value = movie.id;
        document.getElementById("editTitle").value = movie.title;
        document.getElementById("editDescription").value = movie.description;
        document.getElementById("editYear").value = movie.year;
        document.getElementById("editGenre").value = movie.genre;

        const modal = new bootstrap.Modal(document.getElementById("editModal"));
        modal.show();
    }

    // Edit movie
    document.getElementById("editMovieForm").addEventListener("submit", async function (e) {
        e.preventDefault();

        const updatedMovie = {
            id: parseInt(document.getElementById("editId").value),
            title: document.getElementById("editTitle").value,
            description: document.getElementById("editDescription").value,
            year: parseInt(document.getElementById("editYear").value),
            genre: parseInt(document.getElementById("editGenre").value)
        };

        const res = await fetch(apiUrl, {
            method: "PUT",
            headers,
            body: JSON.stringify(updatedMovie)
        });

        if (res.status === 204) {
            loadMovies();
            bootstrap.Modal.getInstance(document.getElementById("editModal")).hide();
        } else {
            alert("Error updating movie.");
        }
    });

    function getGenreName(genreEnum) {
        switch (genreEnum) {
            case 1: return "Comedy";
            case 2: return "Action";
            case 3: return "Romance";
            case 4: return "ScienceFiction";
            default: return "Unknown";
        }
    }

    // Init
    loadMovies();