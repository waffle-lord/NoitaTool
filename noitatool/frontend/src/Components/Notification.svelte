<script>
    import { createEventDispatcher } from "svelte";
    import { scale } from "svelte/transition";

    const dispatch = createEventDispatcher();

    export let type = "error";
</script>

<article class={type} role="alert" transition:scale={{start: .8, duration: 200}}>
    {#if type === "success"}
        <svg width="24" height="24" fill="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M12 2c5.523 0 10 4.477 10 10s-4.477 10-10 10S2 17.523 2 12 6.477 2 12 2Zm3.22 6.97-4.47 4.47-1.97-1.97a.75.75 0 0 0-1.06 1.06l2.5 2.5a.75.75 0 0 0 1.06 0l5-5a.75.75 0 1 0-1.06-1.06Z"/></svg>
    {:else if type === "error"}
        <svg width="24" height="24" fill="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M10.909 2.782a2.25 2.25 0 0 1 2.975.74l.083.138 7.759 14.009a2.25 2.25 0 0 1-1.814 3.334l-.154.006H4.242A2.25 2.25 0 0 1 2.2 17.812l.072-.143L10.03 3.66a2.25 2.25 0 0 1 .879-.878ZM12 16.002a.999.999 0 1 0 0 1.997.999.999 0 0 0 0-1.997Zm-.002-8.004a1 1 0 0 0-.993.884L11 8.998 11 14l.007.117a1 1 0 0 0 1.987 0l.006-.117L13 8.998l-.007-.117a1 1 0 0 0-.994-.883Z"/></svg>
    {:else}
        <svg width="24" height="24" fill="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg"><path d="M12 1.999c5.524 0 10.002 4.478 10.002 10.002 0 5.523-4.478 10.001-10.002 10.001-5.524 0-10.002-4.478-10.002-10.001C1.998 6.477 6.476 1.999 12 1.999Zm-.004 8.25a1 1 0 0 0-.992.885l-.007.116.003 5.502.007.117a1 1 0 0 0 1.987-.002L13 16.75l-.003-5.501-.007-.117a1 1 0 0 0-.994-.882ZM12 6.5a1.251 1.251 0 1 0 0 2.503A1.251 1.251 0 0 0 12 6.5Z"/></svg>
    {/if}

    <div class="text">
        <slot />
    </div>
</article>

<style lang="postcss">
    article {
        color: white;
        padding: 0.75rem 1.5rem;
        border-radius: 0.2rem;
        display: flex;
        align-items: center;
        margin: 0 auto 0.5rem auto;
        border-radius: 7px;
        width: 20rem;
    }
    .error {
        background: IndianRed;
    }
    .success {
        background: MediumSeaGreen;
    }
    .info {
        background: SkyBlue;
    }
    .text {
        margin-left: 1rem;
        user-select: none;
    }
</style>
