async function getCountries() {
    try {
        const response = await fetch('https://localhost:7172/api/Country/GetCountries', {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
        });

        if (response.ok) {
            const data = await response.json();

            const countryTable = document.getElementById('countryTable');
            countryTable.innerHTML = '';

            data.forEach(country => {
                const row = document.createElement('tr');
                row.innerHTML = `
                <td>${country.countryUid}</td>
                <td>${country.name}</td>
                <td style="text-align: center;"><input type="checkbox" value="${country.countryUid}"></td>
            `;
                countryTable.appendChild(row);
            });
        } else {
            throw new Error('Что-то пошло не так');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function createCountry() {
    const countryName = document.getElementById('countryName').value;

    if (!countryName) {
        alert('Please enter the country name');
        return;
    }

    const data = {
        name: countryName
    }

    try {
        const response = await fetch('https://localhost:7172/api/Country/CreateCountry', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`
            },
            body: JSON.stringify(data)
        });

        if (response.ok) {
            const responseData = await response.text();
            console.log(responseData);
            getCountries();
        } else {

        }
    } catch (error) {
        console.error(error);
        alert('Error');
    }
}

async function updateCountry() {
    const countryName = document.getElementById('countryName').value;
    const selectedCheckboxes = document.querySelectorAll('#countryTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length !== 1) {
        alert('Выберите одну страну');
        return;
    }

    if (!countryName) {
        alert('Введите название страны');
        return;
    }

    const uid = selectedCheckboxes[0].value;

    const data = {
        name: countryName
    }

    try {
        const response = await fetch(`https://localhost:7172/api/Country/UpdateCountry?countryUid=${uid}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('userToken')}`,
            },
            body: JSON.stringify(data)
        });

        if (response.ok) {
            const data = await response.text();

            console.log(data);
            alert('Страна изменена');
            getCountries();
        } else {
            throw new Error('Не удалось изменить страну');
        }
    } catch (error) {
        console.error(error);
        alert('Ошибка');
    }
}

async function deleteCountry() {
    const selectedCheckboxes = document.querySelectorAll('#countryTable input[type="checkbox"]:checked');

    if (selectedCheckboxes.length === 0) {
        alert('Выберите хотя бы одну страну');
        return;
    }

    let uids = [];
    for (let i = 0; i < selectedCheckboxes.length; i++) {
        if (selectedCheckboxes[i].checked) {
            uids.push(selectedCheckboxes[i].value);
        }
    }

    uids.forEach(async (uid) => {
        try {
            const response = await fetch(`https://localhost:7172/api/Country/DeleteCountry?countryUid=${uid}`, {
                method: 'DELETE',
                headers: {
                    'Content-Type': 'application/json',
                    Authorization: `Bearer ${localStorage.getItem('userToken')}`
                },
            });

            if (response.ok) {
                const data = await response.text();

                if (data) {
                    //alert(data);
                    getCountries();
                } else {
                    alert('Не получилось удалить страну');
                }
            } else {
                throw new Error('Что-то пошло не так');
            }
        } catch (error) {
            console.error(error);
            alert('Ошибка');
        }
    });
}
