async function getCountries() {
    try {
        const response = await fetch(
            "https://localhost:7172/api/Country/GetCountries", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("userToken")}`,
            },
        }
        );

        if (response.ok) {
            const countries = await response.json();

            const countryTable = document.getElementById("countryTable");
            countryTable.innerHTML = "";

            for (const country of countries) {
                const row = document.createElement("tr");
                row.innerHTML = `
                <td>${country.countryUid}</td>
                <td>${country.name}</td>
                <td style="text-align: center;"><input type="checkbox" value="${country.countryUid}"></td>
            `;
                countryTable.appendChild(row);
            }
        } else {
            console.log(await response.text());
            location.reload();
            throw new Error("Что-то пошло не так");
        }
    } catch (error) {
        console.error(error);
    }
}

async function createCountry() {
    const countryName = document.getElementById("countryName").value;

    if (!countryName) {
        alert("Введите название страны");
        return;
    }

    try {
        const response = await fetch(
            `https://localhost:7172/api/Country/CreateCountry?name=${countryName}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("userToken")}`,
            },
        }
        );

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            await getCountries();
        } else {
            console.log(data);
            throw new Error("Что-то пошло не так");
        }
    } catch (error) {
        console.error(error);
        alert("Ошибка");
    }
}

async function updateCountry() {
    const countryName = document.getElementById("countryName").value;
    const selectedCheckboxes = document.querySelectorAll(
        '#countryTable input[type="checkbox"]:checked'
    );

    if (selectedCheckboxes.length !== 1) {
        alert("Выберите одну страну");
        return;
    }

    if (!countryName) {
        alert("Введите название страны");
        return;
    }

    const uid = selectedCheckboxes[0].value;

    try {
        const response = await fetch(
            `https://localhost:7172/api/Country/UpdateCountry?countryUid=${uid}&name=${countryName}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${localStorage.getItem("userToken")}`,
            },
        }
        );

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            await getCountries();
        } else {
            console.log(data);
            throw new Error("Не удалось изменить страну");
        }
    } catch (error) {
        console.error(error);
        alert("Ошибка");
    }
}

async function deleteCountry() {
    const selectedCheckboxes = document.querySelectorAll(
        '#countryTable input[type="checkbox"]:checked'
    );

    if (selectedCheckboxes.length === 0) {
        alert("Выберите хотя бы одну страну");
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
            const response = await fetch(
                `https://localhost:7172/api/Country/DeleteCountry?countryUid=${uid}`, {
                method: "DELETE",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem("userToken")}`,
                },
            }
            );

            const data = await response.text();

            if (response.ok) {
                console.log(data);
                await getCountries();
            } else {
                console.log(data);
                throw new Error("Что-то пошло не так");
            }
        } catch (error) {
            console.error(error);
            alert("Ошибка");
        }
    }
}