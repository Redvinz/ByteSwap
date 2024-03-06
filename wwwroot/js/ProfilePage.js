function showEditForm(i) {
    let viewFormElements = document.getElementsByClassName("gv-" + i);
    let editFormElements = document.getElementsByClassName("ge-" + i);

    for (let j = 0; j < viewFormElements.length; j++) {
        viewFormElements[j].style.display = 'none';
        editFormElements[j].style.display = 'flex';
    }

    editFormElements[5].style.display = 'flex';

}

function showViewForm(i) {
    let viewFormElements = document.getElementsByClassName("gv-" + i);
    let editFormElements = document.getElementsByClassName("ge-" + i);

    for (let j = 0; j < viewFormElements.length; j++) {
        viewFormElements[j].style.display = 'flex';
        editFormElements[j].style.display = 'none';
    }

    editFormElements[5].style.display = 'none';
}

function toggleNameForm(i) {
    let nameFormContainer = document.getElementById("nameForm-container");
    let nameViewContainer = document.getElementById("nameView-container");

    nameFormContainer.style.display = ((i == 0) ? "none" : "flex")
    nameViewContainer.style.display = ((i == 1) ? "flex" : "none")
}

function showPiViewForm(i) {
    let namePiViewForm = document.getElementsByClassName("piV-" + i);
    let namePiEditForm = document.getElementsByClassName("piF-" + i);

    for (let j = 0; j < namePiViewForm.length; j++) {
        namePiViewForm[j].style.display = 'flex';
        namePiEditForm[j].style.display = 'none';
    }

    namePiEditForm[5].style.display = 'none';
}

function showPiEditForm(i) {
    let namePiViewForm = document.getElementsByClassName("piV-" + i);
    let namePiEditForm = document.getElementsByClassName("piF-" + i);

    for (let j = 0; j < namePiViewForm.length; j++) {
        namePiViewForm[j].style.display = 'none';
        namePiEditForm[j].style.display = 'flex';
    }

    namePiEditForm[5].style.display = 'flex';
}

function showNCMForm(i) {
    let NCMForm = document.getElementById("ncm-form-container");
    let NCMView = document.getElementById("ncm-img-container");

    if (i == 1) {
        NCMView.style.display = "none";
        NCMForm.style.display = "flex";
    } else {
        NCMView.style.display = "flex";
        NCMForm.style.display = "none";
    }
}

function togglePfpEdit(i) {
    let pfpEdit = document.getElementById("pfp-edit");

    let pfpSave = document.getElementById("pfp-save");
    let pfpCancel = document.getElementById("pfp-cancel");

    switch (i) {
        case 0: {
            pfpEdit.style.display = "inline";
            pfpSave.style.display = "none";
            pfpCancel.style.display = "none";
            break;
        }
        case 1: {
            pfpEdit.style.display = "none";
            pfpSave.style.display = "inline";
            pfpCancel.style.display = "inline";
            break;
        }
    }
}