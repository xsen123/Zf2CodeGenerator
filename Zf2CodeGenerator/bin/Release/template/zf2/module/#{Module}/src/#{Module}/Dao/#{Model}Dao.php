<?php
/**
 * Created by Zf2CG
 * User: 
 * Date: #{Date}
 * Time: #{Time}
 */

namespace #{Module}\Dao;

use Zf2Helper\Dao\AbstractBaseDao;

class #{Model}Dao extends AbstractBaseDao{
	
	protected function _provideObjectPrototype() {
		// 注意：这里需要返回一个Model对象，不是字符串
		return new \#{Module}\Model\#{Model}();
	}
	
	protected function _provideTableName() {
		return '#{table}';
	}
	
}
