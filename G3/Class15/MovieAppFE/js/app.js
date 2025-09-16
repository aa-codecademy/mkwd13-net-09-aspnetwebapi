const apiUrl = 'http://localhost:5236/api/Users';

        // LOGIN
        document.getElementById('loginForm').addEventListener('submit', async function (e) {
            e.preventDefault();
            const username = document.getElementById('loginUsername').value;
            const password = document.getElementById('loginPassword').value;

            const response = await fetch(`${apiUrl}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username, password })
            });

            if (response.ok) {
                const data = await response.json();
                localStorage.setItem('user', JSON.stringify(data.token));
                window.location.href = 'movie.html';
            } else {
                const errorText = await response.text();
                document.getElementById('loginError').innerText = errorText;
            }
        });

        // REGISTER
        document.getElementById('registerForm').addEventListener('submit', async function (e) {
            e.preventDefault();

            const username = document.getElementById('regUsername').value;
            const password = document.getElementById('regPassword').value;
            const confirmPassword = document.getElementById('regConfirmPassword').value;
            const firstName = document.getElementById('regFirstName').value;
            const lastName = document.getElementById('regLastName').value;

            const response = await fetch(`${apiUrl}/register`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ username, password, confirmPassword, firstName, lastName })
            });

            const result = await response.json();

            const msgBox = document.getElementById('registerMessage');
            if (response.ok) {
                msgBox.innerHTML = `<span class="text-success">${result.success}</span>`;
            } else {
                msgBox.innerHTML = `<span class="text-danger">${result.error || result}</span>`;
            }
        });