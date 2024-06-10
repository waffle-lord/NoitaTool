<script>
	import { createEventDispatcher } from 'svelte';
	import {scale} from 'svelte/transition';

	export let showModal = false;
	export let message = "";
	export let cancelText = "Cancel";
	export let confirmText = "Confirm";
	export let dialogColor = "orangered";

    const dispatch = createEventDispatcher();
	const delay = (ms) => new Promise((res) => setTimeout(res, ms));

	let dialog;

	$: if (dialog && showModal) dialog.showModal();

	async function cancelModal() {
		showModal = false;
		if (dialog) {
			await delay(500);
			dialog.close();
			dispatch('cancel');
		}
	}

	async function closeModal() {
		showModal = false;
		if (dialog) {
			await delay(500);
			dialog.close();
			dispatch('confirm');
		}
	}
</script>
<!-- svelte-ignore a11y-click-events-have-key-events a11y-no-noninteractive-element-interactions -->
<dialog class="fadeIn" style="border-color: {dialogColor};" bind:this={dialog} transition:scale={{start: .9, duration: 200}}>
	<div class="msg-text">
		<p>{message}</p>
	</div>
	<div class="button-cluster">
		<button class="button-secondary action-button" on:click={cancelModal}>{cancelText}</button>
	    <button class="button-main action-button" style="background-color: {dialogColor};" on:click={closeModal}>{confirmText}</button>
	</div>
</dialog>

<style>
	
	@import "../assets/Styles/ButtonStyles.css";

	dialog {
        width: 80%;
        height: 30%;
		border-radius: 7px;
		border-width: 1px 2px;
		border-style: dashed solid;
		padding: 0;
        background-color: #1d1d1d;
		overflow: hidden;
		box-shadow: 0 0 0 100vw rgba(0, 0, 0, 0);
        transition:  2s ease-out;
	}

	dialog::backdrop {
		background-color: transparent;
	}

	.fadeIn {
        box-shadow: 0 0 0 100vw  rgba(0, 0, 0, 0.527);
        transition:  2s ease-out;
    }
	
	dialog > div {
		padding: 1em;
	}

	.button-cluster {
		position: absolute;
		margin-right: 0px;
		padding: 0;
		right: 10px;
		bottom: 10px;
	}

	.action-button {
		height: 30px;
		margin-left: 5px;
	}

	.msg-text {
		color: white;
	}
</style>