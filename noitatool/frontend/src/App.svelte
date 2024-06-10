<script>
    import {BackupSave, GetBackups} from '../wailsjs/go/main/App.js';
    import BackupInfo from './Components/BackupInfo.svelte';
  
  let buttonText = "Backup";
  let newName = "";
  let backups;

  async function backup() {
    buttonText = "backing up save ...";
    buttonText = await BackupSave(newName);
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
    {#each backups as backup}
      <BackupInfo backup={backup} on:deleted={getBackups}/>
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

  .list {
    display: flex;
    margin: 10px;
    flex-flow: column;
    overflow: auto;
  }

</style>
