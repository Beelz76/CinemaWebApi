async function getAllScreenings() {
    try {
        const response = await fetch('https://localhost:7172/api/Screening/GetAllScreenings', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const screenings = await response.json();

            const screeningTable = document.getElementById('screeningTable');
            screeningTable.innerHTML = '';

            for (const screening of screenings) {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${screening.screeningUid}</td>
                <td>${screening.movieTitle}</td>
                <td>${screening.movieDuration}</td>
                <td>${screening.screeningStart}</td>
                <td>${screening.screeningEnd}</td>
                <td>${screening.hallName}</td>
                <td>${screening.price}</td>
                <td style="text-align: center;"><input type="checkbox" value="${screening.screeningUid}"></td>`;
                screeningTable.appendChild(row);
            }
        } else {
            console.log(await response.text())
            location.reload();
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
    }
}

async function getHallScreenings() {
    const hallName = document.getElementById('hallName').value;

    if (!hallName) {
        alert('Введите название зала');
        return;
    }

    try {
        const response = await fetch(`https://localhost:7172/api/Screening/GetHallScreenings?hallName=${hallName}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const screenings = await response.json();

            const screeningTable = document.getElementById('screeningTable');
            screeningTable.innerHTML = '';

            for (const screening of screenings) {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${screening.screeningUid}</td>
                <td>${screening.movieTitle}</td>
                <td>${screening.movieDuration}</td>
                <td>${screening.screeningStart}</td>
                <td>${screening.screeningEnd}</td>
                <td>${screening.hallName}</td>
                <td>${screening.price}</td>
                <td style="text-align: center;"><input type="checkbox" value="${screening.screeningUid}"></td>`;
                screeningTable.appendChild(row);
            }
        } else {
            console.log(await response.text())
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        //alert('Ошибка');
    }
}

async function getMovieScreenings() {
    const uid = localStorage.getItem('selectedMovie');

    if (!uid) {
        alert('Выберите фильм');
        window.location.href = 'userMovie.html';
        return;
    }

    try {
        const response = await fetch(`https://localhost:7172/api/Screening/GetMovieScreenings?movieUid=${uid}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const screenings = await response.json();

            const screeningTable = document.getElementById('screeningTable');
            screeningTable.innerHTML = '';

            for (const screening of screenings) {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${screening.screeningStart}</td>
                <td>${screening.screeningEnd}</td>
                <td>${screening.hallName}</td>
                <td>${screening.price}</td>
                <td style="text-align: center;"><input type="checkbox" value="${screening.screeningUid}"></td>`;
                screeningTable.appendChild(row);
            }
        } else {
            console.log(await response.text())
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        //alert('Ошибка');
    }
}

async function createScreening() {
    const movieTitle = document.getElementById('movieTitle').value;
    const hallName = document.getElementById('hallName').value;
    const screeningStart = document.getElementById("screeningStart").value;
    const price = document.getElementById("screeningPrice").value;

    if (!movieTitle || !hallName || !screeningStart || !price) {
        alert('Заполните необходимые поля');
        return;
    }

    const screeningInfo = {
        movieTitle: movieTitle,
        hallName: hallName,
        screeningStart: screeningStart,
        price: parseInt(price),
    };

    try {
        const response = await fetch(`https://localhost:7172/api/Screening/CreateScreening`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`
            },
            body: JSON.stringify(screeningInfo)
        });

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            await getAllScreenings();
        } else {
            console.log(data);
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function updateScreening() {
    const movieTitle = document.getElementById('movieTitle').value;
    const hallName = document.getElementById('hallName').value;
    const screeningStart = document.getElementById("screeningStart").value;
    const price = document.getElementById("screeningPrice").value;
    const selectedCheckboxes = document.querySelectorAll('#screeningTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length !== 1) {
        alert('Выберите один сеанс');
        return;
    }

    if (!movieTitle || !hallName || !screeningStart || !price) {
        alert('Заполните необходимые поля');
        return;
    }

    const screeningInfo = {
        movieTitle: movieTitle,
        hallName: hallName,
        screeningStart: screeningStart,
        price: parseInt(price),
    };

    const uid = selectedCheckboxes[0].value;

    try {
        const response = await fetch(`https://localhost:7172/api/Screening/UpdateScreening?screeningUid=${uid}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`
            },
            body: JSON.stringify(screeningInfo)
        });

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            await getAllScreenings();
        } else {
            console.log(data);
            throw new Error('Не удалось изменить сеанс');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function deleteScreening() {
    const selectedCheckboxes = document.querySelectorAll('#screeningTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы один сеанс');
        return;
    }

    let uids = [];
    for (let i = 0; i < selectedCheckboxes.length; i++) {
        if (selectedCheckboxes[i].checked) {
            uids.push(selectedCheckboxes[i].value);
        }
    }

    for (const uid of uids) {
        try {
            const response = await fetch(`https://localhost:7172/api/Screening/DeleteScreening?screeningUid=${uid}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            const data = await response.text();

            if (response.ok) {
                console.log(data);
                await getAllScreenings();
            } else {
                console.log(data);
                throw new Error('Что-то пошло не так');
            }
        } catch (error) {
            console.error(error);
            alert('Ошибка');
        }
    }
}