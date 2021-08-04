const allDeleteDivElements = document
    .querySelectorAll(".delete-btn");

for (let deleteBtn of allDeleteDivElements) {
    deleteBtn
        .addEventListener("click", showSweetAlertDialog);
}

function showSweetAlertDialog(e) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            deleteComment(e);
        }
    });
}

async function deleteComment(e) {
    const token = document
        .getElementsByName("__RequestVerificationToken")[0].value;

    const parentDivNode = e.target
        .parentNode;

    let commentId = parentDivNode
        .querySelector("input[name=commentId]").value;

    let result = await fetch("/api/comments/delete", {
        method: 'DELETE',
        headers: {
            'Content-Type': 'application/json',
            'X-CSRF-TOKEN': token,
        },
        body: JSON.stringify({ commentId }),
    }).then(x => x.text());

    if (!result) {
        Swal.fire(
            'Unsuccessful!',
            'Something went wrong. We are sorry. Comment is not deleted!',
            'error',
        );

        return;
    }

    let currCommentsContainer = parentDivNode
        .querySelector("div")
        .closest(".container-fluid");

    currCommentsContainer.remove();

    Swal.fire(
        'Deleted!',
        'Your file has been deleted.',
        'success'
    );
}