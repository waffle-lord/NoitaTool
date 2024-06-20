<script>
  import { createEventDispatcher } from "svelte";
  import {slide} from 'svelte/transition';
  import { RestoreSave, DeleteSave } from "../../wailsjs/go/main/App.js";
  import ConfirmModal from "./ConfirmModal.svelte";
  import {addNotification, dismissNotification, updateNotification} from '../store.js';


  export let backup;
  let showRestoreModal = false;
  let showDeleteModal = false;
  let showButtons = false;

  const dispatch = createEventDispatcher();

  function getHealthInfo() {
    let hp = Math.floor(backup.Player.Health.CurrentHp * 25);
    let maxHp = Math.floor(backup.Player.Health.MaxHp * 25);

    return `${hp}/${maxHp} HP`;
  }

  function getKillsInfo() {
    let kills = backup.Player.Stats.KillStats.Kills;

    if (kills == 1) {
      return `${kills} kill`;
    }

    return `${kills} kills`;
  }

  async function restoreSave() {
    const id = addNotification({type: "info", message: `Restoring '${backup.Name}' ...`});
    const restored = RestoreSave(backup.Name);

    const type = restored ? "success" : "error";
    const message = restored ? `Backup Restored '${backup.Name}'` : `Restore failed '${backup.Name}'`;

    updateNotification(id, {type: type, message: message}, true);
  }

  async function deleteSave() {
    const id = addNotification({type: "info", message: `Deleting '${backup.Name}' ...`});

    const removed = await DeleteSave(backup.Name);

    if (!removed) {
      updateNotification(id, {type: "error", message: `Deletion failed '${backup.Name}'`}, true)
      return;
    }

    updateNotification(id, {type: "success", message: `Deleted '${backup.Name}'`}, true)

    dispatch("deleted");
  }
</script>

{#if showRestoreModal}
  <ConfirmModal
    bind:showModal={showRestoreModal}
    confirmText="Restore Backup"
    dialogColor="cornflowerblue"
    message="Restoring a backup will delete your current save data"
    on:confirm={restoreSave}
  />
{/if}

{#if showDeleteModal}
  <ConfirmModal
    bind:showModal={showDeleteModal}
    confirmText="Delete Backup"
    dialogColor="red"
    message="Are you sure you want to delete backup '{backup.Name}'"
    on:confirm={deleteSave}
  />
{/if}

<div on:mouseenter={(_) => showButtons = true} on:mouseleave={(_) => showButtons = false}>
  {#if showButtons}
  <div class="buttons-row" transition:slide={{duration: 100}}>
    <button
      class="card-button restore-button"
      on:click={() => (showRestoreModal = true)}>Restore Backup</button
    >
    <button
      class="card-button delete-button"
      on:click={() => (showDeleteModal = true)}>Delete Backup</button
    >
  </div>
  {/if}
  <div class="title">
      <p>{backup.Name}</p>
  </div>

  <div class="icons">
    <div class="icon-info">
      <div>
        <svg
          class="health"
          width="24"
          height="24"
          fill="currentColor"
          viewBox="0 0 24 24"
          xmlns="http://www.w3.org/2000/svg"
          ><path
            d="m12.82 5.58-.82.822-.824-.824a5.375 5.375 0 1 0-7.601 7.602l7.895 7.895a.75.75 0 0 0 1.06 0l7.902-7.897a5.376 5.376 0 0 0-.001-7.599 5.38 5.38 0 0 0-7.611 0Z"
          />
        </svg>
      </div>
      <div>
        <p class="health">{getHealthInfo()}</p>
      </div>
    </div>
    <div class="icon-info">
      <div>
        <svg
          class="gold"
          width="24"
          height="24"
          fill="currentColor"
          viewBox="0 0 24 24"
          xmlns="http://www.w3.org/2000/svg"
          ><path
            d="M4 4.5A2.5 2.5 0 0 1 6.5 2H18a2.5 2.5 0 0 1 2.5 2.5v14.25a.75.75 0 0 1-.75.75H5.5a1 1 0 0 0 1 1h13.25a.75.75 0 0 1 0 1.5H6.5A2.5 2.5 0 0 1 4 19.5v-15Zm6.197 2.964C9.622 7.739 9 8.24 9 9s.622 1.26 1.197 1.536c.622.297 1.437.464 2.303.464.866 0 1.681-.167 2.303-.464C15.378 10.261 16 9.76 16 9s-.621-1.26-1.197-1.536C14.18 7.167 13.366 7 12.5 7c-.866 0-1.681.167-2.303.464Zm5.798 3.426C15.17 11.567 13.91 12 12.5 12c-1.41 0-2.67-.433-3.495-1.11A1.163 1.163 0 0 0 9 11c0 1.105 1.567 2 3.5 2s3.5-.895 3.5-2c0-.037-.002-.073-.005-.11ZM12.5 14c-1.41 0-2.67-.433-3.495-1.11A1.166 1.166 0 0 0 9 13c0 1.105 1.567 2 3.5 2s3.5-.895 3.5-2a1.15 1.15 0 0 0-.005-.11C15.17 13.567 13.91 14 12.5 14Z"
          />
        </svg>
      </div>
      <div>
        <p class="gold">{backup.Player.Wallet.Money} gold</p>
      </div>
    </div>
    <div class="icon-info">
      <div>
        <svg
          class="kills"
          width="24"
          height="24"
          fill="currentColor"
          viewBox="0 0 24 24"
          xmlns="http://www.w3.org/2000/svg"
          ><path
            d="M 10.711383,27.357651 C 9.3877695,27.274605 8.1487087,26.94887 6.999153,26.381745 6.0781454,25.927372 5.8320982,25.602853 5.5922703,24.526165 5.2581831,23.026311 4.73523,22.151079 3.985417,21.836881 3.9162714,21.807905 3.4212531,21.765859 2.8853758,21.743441 1.7544613,21.696132 1.5468805,21.629488 1.0046975,21.139652 0.58869631,20.763816 0.29645797,20.318657 0.1166426,19.786902 0.00705491,19.462828 -0.0139756,19.282458 0.00753411,18.85114 0.03850636,18.230079 0.10357051,18.080107 0.76833303,17.097511 1.3368788,16.257134 1.502467,15.809912 1.502467,15.114762 c 0,-0.637836 -0.144461,-1.144179 -0.5662876,-1.984864 C 0.52912142,12.318643 0.43681549,11.971511 0.39321709,11.087992 0.18814966,6.9323237 2.5862168,2.7795034 6.1814734,1.0642623 7.0801229,0.63552927 8.0213259,0.34258699 9.1398953,0.14347424 9.8981393,0.00850312 12.875933,-0.0491621 13.985325,0.04964234 c 3.867186,0.34442094 6.779829,2.31827646 8.496785,5.75815246 0.779091,1.5608906 1.111707,3.0187264 1.111496,4.8716112 -1.48e-4,1.270642 -0.0472,1.488731 -0.527949,2.446838 -0.423651,0.844325 -0.568121,1.349989 -0.568121,1.988518 0,0.69515 0.165591,1.142372 0.734137,1.982749 0.66476,0.982596 0.729824,1.132568 0.760797,1.753629 0.0215,0.431318 4.75e-4,0.611688 -0.109108,0.935762 -0.175203,0.518118 -0.467601,0.968119 -0.875456,1.34734 -0.522415,0.485738 -0.758585,0.56173 -1.893278,0.609199 -0.535876,0.02243 -1.030894,0.06446 -1.100041,0.09344 -0.754305,0.31608 -1.270764,1.180443 -1.606853,2.689284 -0.164082,0.736636 -0.30146,1.042301 -0.603679,1.343185 -0.448631,0.446652 -1.941362,1.073257 -3.117731,1.308736 -0.918597,0.183879 -2.655558,0.262345 -3.974941,0.179565 z m 0.524287,-3.731633 c 0.161088,-0.07095 0.398961,-0.213911 0.528608,-0.317681 l 0.235724,-0.188676 0.235724,0.188676 c 0.288333,0.230782 0.720568,0.426398 1.063266,0.481198 0.325244,0.05201 0.643453,-0.09005 0.801669,-0.357885 0.173387,-0.293527 0.202044,-0.908306 0.06295,-1.350524 -0.144698,-0.460027 -0.847502,-1.85799 -1.234483,-2.455535 -0.351905,-0.543384 -0.481202,-0.630383 -0.742863,-0.49985 -0.16435,0.08199 -0.208178,0.08199 -0.372528,0 -0.143905,-0.07179 -0.216321,-0.07684 -0.318425,-0.02219 -0.183755,0.09834 -0.640926,0.823271 -1.130104,1.791984 -0.5348332,1.059118 -0.6535591,1.436463 -0.6125824,1.946947 0.0715,0.890737 0.6026104,1.171339 1.4830434,0.783539 z m -3.4483981,-5.06062 c 1.4444552,-0.234253 2.3601791,-1.067711 2.8284591,-2.574365 0.091,-0.2928 0.138179,-0.613218 0.140996,-0.957701 0.0038,-0.459449 -0.01827,-0.563381 -0.195729,-0.923869 -0.448369,-0.910772 -1.4182353,-1.402017 -3.2315324,-1.6368 -1.2715871,-0.164645 -2.0674114,-0.09464 -2.7276815,0.239946 -0.8247761,0.417947 -1.3005731,1.455227 -1.3019675,2.838408 -0.00169,1.67485 0.7544316,2.69336 2.2387722,3.015676 0.5362457,0.116443 1.5261995,0.115872 2.2486831,-0.0012 z m 10.6454431,0.0039 c 1.511524,-0.320261 2.269176,-1.328804 2.267473,-3.018332 -0.0014,-1.383181 -0.477192,-2.420461 -1.301967,-2.838408 -0.660271,-0.334583 -1.456095,-0.404591 -2.727683,-0.239946 -1.813297,0.234783 -2.783162,0.726028 -3.231533,1.6368 -0.177467,0.360488 -0.199486,0.464419 -0.195727,0.923869 0.01098,1.342011 0.810332,2.683585 1.915772,3.215281 0.78633,0.378209 2.301691,0.526677 3.273665,0.320736 z"
          />
        </svg>
      </div>
      <div>
        <p class="kills">{getKillsInfo()}</p>
      </div>
    </div>
  </div>
</div>

<style>

  .title > p {
    user-select: none;
    margin: 10px;
    font-size: 22px;
  }

  .buttons-row {
    display: flex;
  }

  .card-button {
    margin: 0px;
    left: 0px;
    width: 50%;
    height: 30px;
    border-style: solid;
    border-width: 1px;
    color: whitesmoke;
    font-size: 16px;
    opacity: 80%;
  }

  .restore-button {
    background-color: dodgerblue;
    border-color: dodgerblue;
    border-radius: 7px 0 0 0;
  }

  .restore-button:hover {
    background-color: rgb(76, 165, 255);
  }

  .restore-button:active {
    background-color: blue;
  }

  .delete-button {
    background-color: crimson;
    border-color: crimson;
    border-radius: 0 7px 0 0;
  }

  .delete-button:hover {
    background-color: rgb(220, 56, 89);
  }

  .delete-button:active {
    background-color: darkred;
  }

  .icons {
    display: flex;
    justify-content: space-evenly;
    align-items: center;
  }

  .icon-info {
    display: flex;
    align-items: center;
  }

  .health {
    color: crimson;
  }

  .gold {
    color: goldenrod;
  }

  .kills {
    color: grey;
  }
</style>
