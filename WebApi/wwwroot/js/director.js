async function getDirectors() {
    try {
        const response = await fetch(
            "https://localhost:7172/api/Director/GetDirectors",
            {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem(
                        "userToken"
                    )}`,
                },
            }
        );

        if (response.ok) {
            const data = await response.json();

            const directorTable = document.getElementById("directorTable");
            directorTable.innerHTML = "";

            data.forEach((director) => {
                const row = document.createElement("tr");
                row.innerHTML = `
                <td>${director.directorUid}</td>
                <td>${director.fullName}</td>
                <td style="text-align: center;"><input type="checkbox" value="${director.directorUid}"></td>`;
                directorTable.appendChild(row);
            });
        } else {
            console.log(await response.text());
            location.reload(true);
            throw new Error("Что-то пошло не так");
        }
    } catch (error) {
        console.error(error);
    }
}

async function createDirector() {
    const directorFullName = document.getElementById("directorFullName").value;

    if (!directorFullName) {
        alert("Введите имя режиссера");
        return;
    }

    try {
        const response = await fetch(
            `https://localhost:7172/api/Director/Createdirector?fullName=${directorFullName}`,
            {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem(
                        "userToken"
                    )}`,
                },
            }
        );

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            getDirectors();
        } else {
            console.log(data);
            throw new Error("Что-то пошло не так");
        }
    } catch (error) {
        console.error(error);
        alert("Ошибка");
    }
}

async function updateDirector() {
    const directorFullName = document.getElementById("directorFullName").value;
    const selectedCheckboxes = document.querySelectorAll(
        '#directorTable input[type="checkbox"]:checked'
    );

    if (selectedCheckboxes.length !== 1) {
        alert("Выберите одного режиссера");
        return;
    }

    if (!directorFullName) {
        alert("Введите имя режиссера");
        return;
    }

    const uid = selectedCheckboxes[0].value;

    try {
        const response = await fetch(
            `https://localhost:7172/api/Director/UpdateDirector?directorUid=${uid}&fullName=${directorFullName}`,
            {
                method: "PUT",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${localStorage.getItem(
                        "userToken"
                    )}`,
                },
            }
        );

        const data = await response.text();

        if (response.ok) {
            console.log(data);
            getDirectors();
        } else {
            console.log(data);
            throw new Error("Не удалось изменить режиссера");
        }
    } catch (error) {
        console.error(error);
        alert("Ошибка");
    }
}

async function deleteDirector() {
    const selectedCheckboxes = document.querySelectorAll(
        '#directorTable input[type="checkbox"]:checked'
    );

    if (selectedCheckboxes.length === 0) {
        alert("Выберите хотя бы одного режиссера");
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
            const response = await fetch(
                `https://localhost:7172/api/Director/DeleteDirector?directorUid=${uid}`,
                {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${localStorage.getItem(
                            "userToken"
                        )}`,
                    },
                }
            );

            const data = await response.text();

            if (response.ok) {
                console.log(data);
                getDirectors();
            } else {
                console.log(data);
                throw new Error("Что-то пошло не так");
            }
        } catch (error) {
            console.error(error);
            alert("Ошибка");
        }
    });
}
