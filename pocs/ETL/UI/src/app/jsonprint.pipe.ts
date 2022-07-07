import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'prettyprint'
})
export class PrettyPrintPipe implements PipeTransform {

  transform(val: any) {
    return JSON.stringify(val, undefined, 4);
  }

}