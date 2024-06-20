<script>
    import Notification from "./Notification.svelte";
    import { notifications } from "../store.js";
    import {flip} from 'svelte/animate';
</script>

{#if $notifications}
    <section>
        {#each $notifications as notification (notification.id)}
            <div animate:flip>
                <Notification type={notification.type}>{notification.message}</Notification>
            </div>
        {/each}
    </section>
{/if}

<style>
    section {
        position: fixed;
        bottom: 5px;
        left: 0;
        right: 0;
        width: 100%;
        display: flex;
        margin-top: 1rem;
        justify-content: center;
        flex-direction: column;
        z-index: 1000;
    }
</style>
