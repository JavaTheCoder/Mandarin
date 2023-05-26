var textarea = document.querySelector('textarea');
textarea.addEventListener('keydown', autosize);

function autosize() {
    var el = this;
    setTimeout(function () {
        el.style.cssText = 'height:auto; padding:0';
        // for box-sizing other than "content-box" use:
        // el.style.cssText = '-moz-box-sizing:content-box';
        el.style.cssText = 'height:' + el.scrollHeight + 'px';
    }, 0);
}

function sendAjaxRequest(message) {
    $.ajax({
        type: "POST",
        url: "/Chat/AddMessage",
        data: { msg: message },
        success: response => {
            window.location.reload();
        },
        error: error => {
            console.log(JSON.stringify(error));
        }
    });
}

function imgClick(id) {
    if (window.location.href.includes("AddToFavorites")) {
        window.location.href = window.location.href.replace("AddToFavorites", "Details");
    } else if (window.location.href.includes("GetFavoriteProducts")) {
        window.location.href = window.location.href.replace("GetFavoriteProducts", `Details/${id}`).split('?')[0];
    } else if (window.location.href.includes("SearchByName")) {
        window.location.href = window.location.href.replace("SearchByName", `Details/${id}`).split('?')[0];
    } else if (window.location.href.includes(`GetProductsByCategory`)) {
        window.location.href = window.location.href.replace(`GetProductsByCategory`, `Details/${id}`).split('?')[0].slice(0, -1);
    } else if (window.location.href.includes(`Chat/OpenChat`)) {
        window.location.href = window.location.href.replace(`Chat/OpenChat`, `Product/Details/${id}`).split('?')[0].slice(0, -1);
    }  else if (window.location.href.includes(`Chat/OpenOrCreateChat`)) {
        window.location.href = window.location.href.replace(`Chat/OpenOrCreateChat`, `Product/Details/${id}`).split('?')[0].slice(0, -1);
    } else {
        if (window.location.href.includes("Product")) {
            window.location.href += `/Details/${id}`;
        } else {
            window.location.href += `Product/Details/${id}`;
        }
    }
}

const API_KEY = "649466253882926";
const URL = "https://api.cloudinary.com/v1_1/df92myoum/image/upload";

document.getElementById("file-upload").addEventListener("change", function (ev) {
    var file = ev.target.files[0];
    const formData = new FormData();

    formData.append("file", file);
    formData.append("api_key", API_KEY);
    formData.append("folder", "mandarin");
    formData.append("upload_preset", "clemdv9v");

    $.ajax({
        type: "POST",
        url: URL,
        data: formData,
        processData: false,
        contentType: false,
        success: response => {
            $.ajax({
                type: "POST",
                url: "/Product/SaveUploadedImage",
                data: { img: response.secure_url },
            });

            console.log(response.secure_url);
        },
        error: error => {
            console.log(JSON.stringify(error));
        }
    });
});
