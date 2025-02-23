let initialProjectName: string
const editProjectNameButtons: NodeListOf<HTMLElement> = document.querySelectorAll(".edit-name-button")
const updateProjectNameButtons: NodeListOf<HTMLElement> = document.querySelectorAll(".update-name-button")
const cancelEditProjectNameButtons: NodeListOf<HTMLElement> = document.querySelectorAll(".cancel-edit-name-button")
const projectNameInputElements: NodeListOf<HTMLInputElement> = document.querySelectorAll(".project-name")
const projectStatusButtons: NodeListOf<HTMLInputElement> = document.querySelectorAll(".switch")
const deleteProjectButtons: NodeListOf<HTMLElement> = document.querySelectorAll(".delete-project")

editProjectNameButtons.forEach(editProjectNameButton => editProjectNameButton.addEventListener("click", () => editProjectName(editProjectNameButton)))
updateProjectNameButtons.forEach(updateProjectNameButton => updateProjectNameButton.addEventListener("click", () => updateProjectName(updateProjectNameButton)))
cancelEditProjectNameButtons.forEach(cancelEditProjectNameButton => cancelEditProjectNameButton.addEventListener("click", () => cancelEditProjectName(cancelEditProjectNameButton)))
projectNameInputElements.forEach(projectNameInputElement => projectNameInputElement.addEventListener("input", () => checkIfProjectNameIsNotEmpty(projectNameInputElement)))
projectStatusButtons.forEach(projectStatusButton => projectStatusButton.addEventListener("click", () => updateProjectStatus(projectStatusButton)))

function deleteProject(deleteProjectButton: HTMLElement) {
    
    const projectId = getProjectId(deleteProjectButton)
    const hiddenInputRemoveProject: HTMLInputElement | null = document.querySelector("#modal-remove-project-id")
    hiddenInputRemoveProject!.value = projectId
}

deleteProjectButtons.forEach(deleteProjectButton => deleteProjectButton.addEventListener("click", () => deleteProject(deleteProjectButton)))

function editProjectName(editProjectNameButton: HTMLElement) {
    
    const projectId = getProjectId(editProjectNameButton)
    const projectNameInputElement = getCorrespondingProjectNameInputElement(projectId)
    const updateProjectNameButton = getCorrespondingUpdateProjectNameButton(projectId)
    const cancelEditProjectNameButton = getCorrespondingCancelEditProjectNameButton(projectId)

    initialProjectName = projectNameInputElement!.value
    
    projectNameInputElement?.removeAttribute("readonly")
    projectNameInputElement?.removeAttribute("disabled")
    projectNameInputElement?.focus()
    projectNameInputElement?.setSelectionRange(projectNameInputElement.value.length, projectNameInputElement.value.length);
    
    editProjectNameButton.style.display = "none"
    updateProjectNameButton!.style.display = "flex"
    cancelEditProjectNameButton!.style.display = "flex"
}

function checkIfProjectNameIsNotEmpty(projectNameInputElement: HTMLInputElement){
    
    if (projectNameInputElement.value.length > 0){

        projectNameInputElement.classList.remove("is-invalid")
    }
}

function getProjectId(element: HTMLElement): string{

    const lastHyphenIndex= element.id.lastIndexOf("-")
    return element.id.substring(lastHyphenIndex + 1)
}

function getCorrespondingProjectNameInputElement(projectId: string) : HTMLInputElement | null{

    return document.querySelector("#project-name-" + projectId)
}

function getCorrespondingEditProjectNameButton(projectId: string): HTMLElement | null{
    
    return document.querySelector("#edit-name-button-" + projectId)
}

function getCorrespondingUpdateProjectNameButton(projectId: string): HTMLElement | null{

    return document.querySelector("#update-name-button-" + projectId)
}

function getCorrespondingCancelEditProjectNameButton(projectId: string): HTMLElement | null{

    return document.querySelector("#cancel-edit-name-button-" + projectId)
}

async function updateProjectName(updateProjectNameButton: HTMLElement) {

    const projectId = getProjectId(updateProjectNameButton)
    const projectNameInputElement = getCorrespondingProjectNameInputElement(projectId)
    const editProjectNameButton = getCorrespondingEditProjectNameButton(projectId)
    const cancelEditProjectNameButton = getCorrespondingCancelEditProjectNameButton(projectId)
    
    const projectName = projectNameInputElement?.value
    
    if (projectName?.trim() !== ""){

        const updatedProject = {projectName: projectName}

        try {
            const response = await fetch(`/api/projects/${projectId}`, {
                method: 'PUT',
                body: JSON.stringify(updatedProject),
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            if (!response.ok) {
                throw new Error(`Received status code ${response.status}.`);
            }

        } catch (error) {
            console.error("Error updating project:", error);
        }

        projectNameInputElement?.setAttribute("readonly", String(true))
        projectNameInputElement?.setAttribute("disabled", String(true))
        projectNameInputElement?.classList.remove("is-invalid")
        
        updateProjectNameButton.style.display = "none"
        cancelEditProjectNameButton!.style.display = "none"
        editProjectNameButton!.style.display = "flex"
    }
    else{
        projectNameInputElement!.value = ""
        projectNameInputElement?.classList.add("is-invalid");
    }
}

function cancelEditProjectName(cancelEditProjectNameButton: HTMLElement){

    const projectId = getProjectId(cancelEditProjectNameButton)
    const projectNameInputElement = getCorrespondingProjectNameInputElement(projectId)
    const updateProjectNameButton = getCorrespondingUpdateProjectNameButton(projectId)
    const editProjectNameButton = getCorrespondingEditProjectNameButton(projectId)

    projectNameInputElement?.setAttribute("readonly", String(true))
    projectNameInputElement?.setAttribute("disabled", String(true))
    projectNameInputElement!.value = initialProjectName
    projectNameInputElement?.classList.remove("is-invalid")

    cancelEditProjectNameButton.style.display = "none"
    updateProjectNameButton!.style.display = "none"
    editProjectNameButton!.style.display = "flex"
}

function updateStatusLabel(projectId: string, projectStatus: boolean){

    const label = document.querySelector("#label-switch-" + projectId)

    if (projectStatus){
        label!.innerHTML = "Actief"
    }
    else{
        label!.innerHTML = "Inactief"
    }

}

async function updateProjectStatus(projectStatusButton: HTMLInputElement) {

    const projectId = getProjectId(projectStatusButton)

    const updatedProjectStatus = {projectStatus: projectStatusButton.checked}

    try {
        const response = await fetch(`/api/projects/${projectId}/status`, {
            method: 'PUT',
            body: JSON.stringify(updatedProjectStatus),
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`Received status code ${response.status}.`);
        }

    } catch (error) {
        console.error("Error updating project:", error);
    }

    updateStatusLabel(projectId, projectStatusButton.checked)
}