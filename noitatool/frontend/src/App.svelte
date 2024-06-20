<script>
    import {flip} from 'svelte/animate';
    import {scale} from 'svelte/transition';
    import {BackupSave, GetBackups} from '../wailsjs/go/main/App.js';
    import BackupInfo from './Components/BackupInfo.svelte';
    import {addNotification, dismissNotification, updateNotification} from './store.js';
    import NotificationsList from './Components/NotificationsList.svelte';
  
  let newName = "";
  let backups;

  async function backup() {
    const id = addNotification({type: "pending", message: `Saving '${newName}' ...`});

    const res = await BackupSave(newName);

    updateNotification(id, {type: (res.Success ? "success" : "error"), message: res.Message}, true)

    await getBackups();

    newName = "";
  }

  async function getBackups() {
    backups = await GetBackups();
  }
</script>

<main>

  <NotificationsList />

  <div class="inputs">
    <form on:submit|preventDefault={() => {}}>
      <input type="text" bind:value={newName}/>
      <button class="button-main backup-button" on:click={backup}>Backup</button>
    </form>
  </div>

  <div class="list">
    {#await getBackups() then}
    {#each backups as backup (backup.Name)}
    <div class="card" transition:scale={{start: .9, duration: 200}} animate:flip={{duration: 300}}>
      <BackupInfo backup={backup} on:deleted={getBackups}/>
    </div>
    {:else}
      <p>no backups found</p>
    {/each}
    {/await}
  </div>

</main>


<style>

  @import "./assets/Styles/ButtonStyles.css";

  .backup-button {
    height: 20px;
  }

  .inputs {
    position: sticky;
    top: 0;
    background-color: #1f3049;
    padding: 10px;
    box-shadow: 0px 2px 3px #111111;
  }

  .card {
    margin-bottom: 10px;
    border-radius: 10px;
    background-color: #111111;
  }

  .card:hover {
    background-color: #1a1a1a;
    box-shadow: 3px 5px 10px black;
  }

  .list {
    height: 90vh;
    margin: 10px;
  }

</style>
