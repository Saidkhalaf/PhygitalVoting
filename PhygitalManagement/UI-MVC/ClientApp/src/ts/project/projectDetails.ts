let initalType: string
let initalName: string

const editFlowButtons: NodeListOf<HTMLElement> = document.querySelectorAll(".edit-flow-button")
const updateFlowButtons: NodeListOf<HTMLElement> = document.querySelectorAll(".update-flow-button")
const cancelEditFlowButtons: NodeListOf<HTMLElement> = document.querySelectorAll(".cancel-edit-flow-button")
const deleteFlowButtons: NodeListOf<HTMLElement> = document.querySelectorAll(".delete-flow")
editFlowButtons.forEach(editFlowButton => editFlowButton.addEventListener("click", () => editFlow(editFlowButton)))
updateFlowButtons.forEach(updateFlowButton => updateFlowButton.addEventListener("click", () => updateFlow(updateFlowButton)))
cancelEditFlowButtons.forEach(cancelEditFlowButton => cancelEditFlowButton.addEventListener("click", () => cancelEditFlow(cancelEditFlowButton)))
deleteFlowButtons.forEach(deleteFlowButton => deleteFlowButton.addEventListener("click", () => deleteFlow(deleteFlowButton)))

function deleteFlow(deleteFlowButton: HTMLElement) {
    
    const flowId = deleteFlowButton.getAttribute("data-flow-id") as string
    const projectId = deleteFlowButton.getAttribute("data-project-id") as string

    const hiddenInputFlowId: HTMLInputElement | null = document.querySelector("#modal-remove-flow-id")
    const hiddenInputProjectId: HTMLInputElement | null = document.querySelector("#modal-remove-project-id")
    
    hiddenInputFlowId!.value = flowId
    hiddenInputProjectId!.value = projectId
}

function editFlow(editFlowButton: HTMLElement) {

    const flowId = getFlowId(editFlowButton)
    enableCorrespondingRadioButtons(flowId)
    makeFlowNameEditable(flowId)

    const updateFlowButton = getCorrespondingUpdateFlowButton(flowId)
    const cancelEditFlowButton = getCorrespondingCancelEditFlowButton(flowId)

    editFlowButton.style.display = "none"
    updateFlowButton!.style.display = "inline-flex"
    cancelEditFlowButton!.style.display = "inline-flex"
}

function makeFlowNameEditable(flowId: string){
    
    const flowName = document.querySelector("#flow-name-" + flowId);
    initalName = flowName!.innerHTML
    flowName!.setAttribute("contenteditable", String(true))
}

function makeFlowNameUnEditable(flowId: string){

    const flowName = document.querySelector("#flow-name-" + flowId);
    flowName!.removeAttribute("contenteditable")
}

function resetFlowName(flowId: string){
    const flowName = document.querySelector("#flow-name-" + flowId);
    flowName!.innerHTML = initalName
}

function getFlowName(flowId: string){
    
    return document.querySelector("#flow-name-" + flowId)?.innerHTML;
}

function getCorrespondingEditFlowButton(flowId: string): HTMLElement | null {

    return document.querySelector("#edit-flow-button-" + flowId)
}

function getCorrespondingUpdateFlowButton(flowId: string): HTMLElement | null {

    return document.querySelector("#update-flow-button-" + flowId)
}

function getCorrespondingCancelEditFlowButton(flowId: string): HTMLElement | null {

    return document.querySelector("#cancel-edit-flow-button-" + flowId)
}

function getSelectedType(flowId: string) {

    const radios: NodeListOf<HTMLInputElement> = document.querySelectorAll(".flow-type-" + flowId)
    for (const radio of radios) {
        if (radio.checked) {
            return parseInt(radio.value)
        }
    }
}

async function updateFlow(updateFlowButton: HTMLElement) {

    const flowId = getFlowId(updateFlowButton)

    const flowType = getSelectedType(flowId)
    const flowName = getFlowName(flowId)

    const updatedFlowType = {flowName: flowName, flowType: flowType}

    try {
        const response = await fetch(`/api/flows/${flowId}`, {
            method: 'PUT',
            body: JSON.stringify(updatedFlowType),
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`Received status code ${response.status}.`);
        }

    } catch (error) {
        console.error("Error updating flow:", error);
    }

    disableCorrespondingRadioButtons(flowId)
    makeFlowNameUnEditable(flowId)

    const editFlowButton = getCorrespondingEditFlowButton(flowId)
    const cancelEditFlowButton = getCorrespondingCancelEditFlowButton(flowId)

    cancelEditFlowButton!.style.display = "none"
    updateFlowButton!.style.display = "none"
    editFlowButton!.style.display = "inline-flex"
}

function cancelEditFlow(cancelEditFlowButton: HTMLElement) {

    const flowId = getFlowId(cancelEditFlowButton)
    
    disableCorrespondingRadioButtons(flowId)
    selectInitalRadio(flowId)
    
    makeFlowNameUnEditable(flowId)
    resetFlowName(flowId)

    const editFlowButton = getCorrespondingEditFlowButton(flowId)
    const updateFlowButton = getCorrespondingUpdateFlowButton(flowId)

    cancelEditFlowButton!.style.display = "none"
    updateFlowButton!.style.display = "none"
    editFlowButton!.style.display = "inline-flex"
}

function getFlowId(element: HTMLElement): string {

    const lastHyphenIndex = element.id.lastIndexOf("-")
    return element.id.substring(lastHyphenIndex + 1)
}

function enableCorrespondingRadioButtons(flowId: string) {

    const radios: NodeListOf<HTMLInputElement> = document.querySelectorAll(".flow-type-" + flowId)
    for (const radio of radios) {
        radio.removeAttribute("disabled")
        if (radio.checked) {
            initalType = radio.value
        }
    }
}

function disableCorrespondingRadioButtons(flowId: string) {

    const radios = document.querySelectorAll(".flow-type-" + flowId)
    for (const radio of radios) {
        radio.setAttribute("disabled", String(true))
    }
}

function selectInitalRadio(flowId: string) {

    const radios: NodeListOf<HTMLInputElement> = document.querySelectorAll(".flow-type-" + flowId)
    for (const radio of radios) {
        if (radio.value == initalType) {
            radio.checked = true
        }
    }
}
