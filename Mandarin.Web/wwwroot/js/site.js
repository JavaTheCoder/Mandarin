function imgClick(id) {
    if (window.location.href.includes("AddToFavorites")) {
        window.location.href = window.location.href.replace("AddToFavorites", "Details");

    } else if (window.location.href.includes("GetFavoriteProducts")) {
        window.location.href = window.location.href.replace("GetFavoriteProducts", `Details/${id}`).split('?')[0];
    } else if (window.location.href.includes("SearchByName")) {
        window.location.href = window.location.href.replace("SearchByName", `Details/${id}`).split('?')[0];
    } else if (window.location.href.includes(`ShowByCategory`)) {
        window.location.href = window.location.href.replace(`ShowByCategory`, `Details/${id}`).split('?')[0].slice(0, -1);
    } else {
        if (window.location.href.includes("Product")) {
            window.location.href += `/Details/${id}`;
        } else {
            window.location.href += `Product/Details/${id}`;
        }
    }
}
