async function uploadImage(inputId) {
    var input = document.getElementById(inputId);
    var profileImage = input.files[0];

    var formData = new FormData();

    formData.append("image", profileImage);

    const response = await fetch("/api/users", {
        method: 'POST',
        body: formData
    });

    if (response.ok) {
        var src = await response.text();
        document
            .getElementById('profile-image')
            .setAttribute('src', src);

        input.value = "";
    }

    if (!response.ok) {
        var error = await response.text();
        document
            .getElementById('my-container')
            .prepend(createBootstrapDivAlertElement(error));

        setTimeout(function () {
            document.getElementById('my-alert-div').style.display = 'none';
        }, 3000); // <-- time in milliseconds

        input.value = "";
    };
}

async function editUsername(inputId) {
    var input = document.getElementById(inputId);
    var username = input.value;
    var json = JSON.stringify({ username });

    const jsonResponse = await fetch('/api/users/username', {
        method: 'Post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: json,
    }).then(res => res.json());

    var username = jsonResponse['username'];
    var errorMessage = jsonResponse['errorMessage'];

    if (errorMessage) {
        document
            .getElementById('my-container')
            .prepend(createBootstrapDivAlertElement(errorMessage));

        setTimeout(function () {
            document.getElementById('my-alert-div').style.display = 'none';
        }, 3000);

        input.value = username;
    }

    if (!errorMessage) {
        input.value = username;
    }
}

function createBootstrapDivAlertElement(text) {
    var div = document.createElement('div');

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