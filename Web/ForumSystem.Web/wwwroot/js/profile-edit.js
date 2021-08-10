async function uploadImage(inputId) {
    const input = document.getElementById(inputId);
    const profileImage = input.files[0];

    const token = document
        .getElementsByName("__RequestVerificationToken")[0].value;

    const formData = new FormData();

    formData.append("image", profileImage);

    const response = await fetch("/api/users/image", {
        method: 'POST',
        headers: {
            'X-CSRF-TOKEN': token,
        },
        body: formData
    });

    if (response.ok) {
        let src = await response.text();
        document
            .getElementById('profile-image')
            .setAttribute('src', src);

        input.value = "";
    }

    if (!response.ok) {
        let error = await response.text();
        document
            .getElementById('my-container')
            .prepend(createBootstrapDivAlertElement(error));

        setTimeout(function () {
            document.getElementById('my-alert-div').style.display = 'none';
        }, 3000);

        input.value = "";
    };
}

async function editUsername(inputId) {
    let input = document.getElementById(inputId);
    let username = input.value;
    let json = JSON.stringify({ username });

    const token = document
        .getElementsByName("__RequestVerificationToken")[0].value;

    const jsonResponse = await fetch('/api/users/username', {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': token,
        },
        body: json,
    }).then(res => res.json());

    username = jsonResponse['username'];
    let errorMessage = jsonResponse['errorMessage'];

    if (errorMessage) {
        document
            .getElementById('my-container')
            .prepend(createBootstrapDivAlertElement(errorMessage));

        setTimeout(function () {
            document.getElementById('my-alert-div').style.display = 'none';
        }, 5000);

        input.value = username;
    }

    if (!errorMessage) {
        input.value = username;
        document.getElementById('dropdownMenuLink').textContent = "Hello " + username + "!";
    }

    let newUrl = "/User/" + username + "/1";

    document.getElementById("my-profile-link").href = newUrl;

    history.replaceState(null, '', newUrl)
}

function createBootstrapDivAlertElement(text) {
    let div = document.createElement('div');

    div.id = 'my-alert-div';

    div.classList.add('alert', 'alert-danger', 'alert-dismissible', 'fade', 'show');
    div.setAttribute('role', 'alert');

    let textNode = document
        .createTextNode(text);

    div.append(textNode);

    let btn = document.createElement('button');

    btn.type = 'button';

    btn.classList.add('close');

    btn.setAttribute('data-dismiss', 'alert');
    btn.setAttribute('aria-label', 'Close');

    btn.innerHTML = '<span aria-hidden="true">&times;</span>';

    div.append(btn);

    return div;
}