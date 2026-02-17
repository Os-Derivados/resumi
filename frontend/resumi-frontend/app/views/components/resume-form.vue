<template>
    <UForm :schema="vm.schema" :state="vm.state"  @submit="(e) => e.preventDefault()">
        <UFormField label="Owner Name" name="ownerName" class="w-full">
            <UInput 
                v-model="vm.state.ownerName" 
                :variant="vm.getVariant('ownerName')"
                @focus="vm.setFocus('ownerName', true)"
                @blur="vm.setFocus('ownerName', false)"
                class="w-full" 
            />
        </UFormField>

        <UFormField label="Email" name="email" class="w-full">
            <UInput 
                v-model="vm.state.email" 
                :variant="vm.getVariant('email')"
                @focus="vm.setFocus('email', true)"
                @blur="vm.setFocus('email', false)"
                class="w-full" 
            />
        </UFormField>

        <UFormField label="Keyword" name="keyword" class="w-full">
            <UInput 
                v-model="vm.state.keyword" 
                :variant="vm.getVariant('keyword')"
                @focus="vm.setFocus('keyword', true)"
                @blur="vm.setFocus('keyword', false)"
                class="w-full" 
            />
        </UFormField>

        <UFormField label="Location" name="location" class="w-full">
            <UInput 
                v-model="vm.state.location" 
                :variant="vm.getVariant('location')"
                @focus="vm.setFocus('location', true)"
                @blur="vm.setFocus('location', false)"
                class="w-full" 
            />
        </UFormField>

        <UFormField label="Phone Number" name="phoneNumber" class="w-full">
            <UInput 
                v-model="vm.state.phoneNumber" 
                :variant="vm.getVariant('phoneNumber')"
                @focus="vm.setFocus('phoneNumber', true)"
                @blur="vm.setFocus('phoneNumber', false)"
                class="w-full" 
            />
        </UFormField>
    </UForm>
</template>

<script setup lang="ts">
import { ResumeFormViewModel } from '../models/resume-form.vm';

const route = useRoute()
const resumeId = Number(route.params.id)

const vm = new ResumeFormViewModel(resumeId)
</script>