<script>
    import { scale } from 'svelte/transition';
    import {Backup_save} from '../wailsjs/go/main/App.js';
  
  let buttonText = "Backup";
  let newName = "";
  let names = ["my game backup","big run!","slugma ligma"];

  async function backup() {
    buttonText = "backing up save ...";
    buttonText = await Backup_save(newName);
    names = [...names, newName];
    newName = "";
  }
</script>

<main>

  <div class="inputs">
    <input type="text" bind:value={newName}/>
    <button on:click={backup}>{buttonText}</button>
  </div>

  <div class="list">
    {#each names as name}
    <div class="card" transition:scale={{start: .9, duration: 200}}>
      <p>{name}</p>
    </div>
    {/each}
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

  .card {
    margin-bottom: 10px;
    border-radius: 10px;
    background-color: #111111;
  }

</style>
