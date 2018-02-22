<?php
/**
 * Created by Zf2CG
 * User: 
 * Date: #{Date}
 * Time: #{Time}
 */

namespace #{Module}\Service;

use Zf2Helper\Service\AbstractBaseService;

class #{Model}Service extends AbstractBaseService {

	protected function _provideDao()
	{
		// 注意：这里需要返回一个Dao对象，不是字符串
		return new \#{Module}\Dao\#{Model}Dao();
	}
	
}