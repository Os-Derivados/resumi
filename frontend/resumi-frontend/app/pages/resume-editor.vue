<script setup lang="ts">

    const items = [
        {
            label: "Arquivo",
            icon: "i-custom-archive",
            slot: "archive"
        },
        {
            label: "Revisão",
            icon: "i-custom-checkmark",
            slot: "revision"
        },
        {
            label: "Marketplace",
            icon: "i-custom-marketplace",
            slot: "marketplace"
        },
        {
            label: "Formatar",
            icon: "i-custom-format",
            slot: "format"
        },
        {
            label: "Ajuda",
            icon: "i-custom-help",
            slot: "help"
        }
    ]

    definePageMeta({
        colorMode: "light"
    })
</script>

<template>
    <UContainer class="flex w-6xl justify-between">
        <UserComponent/>
        <UTabs :items="items" >
            <template #archive></template>
            <template #revision></template>
            <template #marketplace></template>
            <template #format></template>
            <template #help></template>
        </UTabs>

    </UContainer>
</template>

