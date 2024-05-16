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
    <button on:click={backup}>{buttonText}</button>
  </div>

  <div class="list">
    {#await getBackups() then}
    {#each backups as backup}
      <BackupInfo name={backup.Name}/>
    {:else}
      <p>no backups found</p>
    {/each}
    {/await}
  </div>

</main>


<style>

  .inputs {
    position: sticky;
    top: 0;
    background-color: #1b2635;
    padding: 10px;
    box-shadow: 0px 4px 5px #111111;
  }

  .list {
    display: flex;
    margin: 10px;
    flex-flow: column;
    overflow: auto;
  }

</style>
