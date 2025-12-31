import type { Reactive } from "vue";

export interface IViewModel<TState> {
	state: Reactive<TState> | ComputedRef<TState>;
}