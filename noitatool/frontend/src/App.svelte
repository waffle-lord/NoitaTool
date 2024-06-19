<script>
    import {flip} from 'svelte/animate';
    import {scale} from 'svelte/transition';
    import {BackupSave, GetBackups} from '../wailsjs/go/main/App.js';
    import BackupInfo from './Components/BackupInfo.svelte';
  
  let buttonText = "Backup";
  let newName = "";
  let backups;

  async function backup() {
    buttonText = "backing up save ...";
    await BackupSave(newName);
    buttonText = "Backup"
    await getBackups()
    newName = "";
  }

  async function getBackups() {
    backups = await GetBackups();
  }
</script>

<main>

  <div class="inputs">
    <input type="text" bind:value={newName}/>
    <button class="button-main backup-button" on:click={backup}>{buttonText}</button>
  </div>

  <div class="list">
    {#await getBackups() then}
    {#each backups as backup (backup.Name)}
    <div class="card" transition:scale={{start: .9, duration: 200}} animate:flip={{duration: 200}}>
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

  .list {
    height: 90vh;
    margin: 10px;
  }

</style>
