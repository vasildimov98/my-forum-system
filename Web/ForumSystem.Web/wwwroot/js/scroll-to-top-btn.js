let scrollToTopBtn = document.getElementById("btn-back-to-top");
let rootElement = document.documentElement;

// When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () {
    scrollFunction();
};

function scrollFunction() {
    if (
        document.body.scrollTop > 20 ||
        document.documentElement.scrollTop > 20
    ) {
        scrollToTopBtn.style.display = "block";
    } else {
        scrollToTopBtn.style.display = "none";
    }
}

// When the user clicks on the button, scroll to the top of the document
scrollToTopBtn.addEventListener("click", backToTop);

function backToTop() {
    document.body.scrollTop = 0;
    rootElement.scrollTop = 0;
}